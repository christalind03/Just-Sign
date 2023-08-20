using UnityEngine;
using UnityEngine.Video;

using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine.SceneManagement;
using TMPro;

public class Gameplay : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer _videoPlayer;

    private UDP_Server _udpServer;
    private Process _pythonClient;
    private bool _runningPredictions;

    private Dictionary<string, string> _songInformation;
    private Queue<(int, string)> _expectedPredictions;

    private int? _expectedFrame;
    private string _expectedPrediction;
    private Dictionary<string, int> _currentPredictions;

    public FeedbackManager feedbackManagerInstance;
    public FeedbackManager feedbackGhostManagerInstance;

    private int _maxScore = 0;
    private int _totalScore = 0;

    public int MaxScore
    {
        get => _maxScore;
    }

    public int TotalScore
    {
        get => _totalScore;
    }

    public void Start()
    {
        _udpServer = UDP_Server.Instance;
        _runningPredictions = false;
        
        _songInformation = new Dictionary<string, string>();
        _expectedPredictions = new Queue<(int, string)>();

        _expectedFrame = null;
        _expectedPrediction = null;
        _currentPredictions = new Dictionary<string, int>();

        _pythonClient = Process.Start(Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Scripts\ASL Recognition\Executable.exe"));
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadSong(string songFolderPath)
    {
        // Load the video into the video player
        string videoFilePath = $"{songFolderPath}/Video.mov";
        _videoPlayer.url = videoFilePath;

        // Load the beatmap
        string beatmapFilePath = $"{songFolderPath}/Beatmap.txt";

        bool readingSongData = false;
        bool readingSignData = false;

        using (StreamReader fileReader = new StreamReader(beatmapFilePath))
        {
            string fileLine;

            while ((fileLine = fileReader.ReadLine()) != null)
            {
                fileLine = Regex.Replace(fileLine, @"/t|/n|/r", "");

                switch (fileLine)
                {
                    case "[Song Information]":
                        readingSongData = true;
                        readingSignData = false;
                        break;
                    
                    case "[Sign Data]":
                        readingSongData = false;
                        readingSignData = true;
                        break;

                    case "":
                        break;

                    default:
                        if (readingSongData == true)
                        {
                            string[] songData = fileLine.Split(new string[] {": "}, StringSplitOptions.None);
                            _songInformation.Add(songData[0], songData[1]);
                        }

                        if (readingSignData == true)
                        {
                            string[] signData = fileLine.Split(new string[] {", "}, StringSplitOptions.None);
                            _expectedPredictions.Enqueue((Convert.ToInt32(signData[0]), signData[1]));
                        }
                        
                        break;
                }
            }
        }

        // Update maximum score based on the number of expected predictions times 1000
        // This score is equivalent to getting "PERFECT" on all predictions
        _maxScore = _expectedPredictions.Count * 1000;

        // Reset the current, total score
        _totalScore = 0;
    }

    public void StartGame()
    {
        _udpServer.SendData("START PREDICTIONS");
        _runningPredictions = true;
    }

    public void Update()
    {
        if (_runningPredictions)
        {
            if (_expectedFrame == null)
            {
                try
                {
                    var expectedInformation = _expectedPredictions.Dequeue();

                    _expectedFrame = expectedInformation.Item1;
                    _expectedPrediction = expectedInformation.Item2;
                }
                catch
                {
                    if ((ulong)_videoPlayer.frame == _videoPlayer.frameCount - 1)
                    {
                        StopGame();
                    }
                }
            }
            else if ((_expectedFrame != null) && (_videoPlayer.frame == _expectedFrame))
            {
                CalculateScore(_expectedPrediction);

                _expectedFrame = null;
                _expectedPrediction = null;
                _currentPredictions.Clear();
            }

            string predictedSign = _udpServer.ReceivedData;

            if (predictedSign != null)
            {
                if (predictedSign != "NO_DETECTIONS")
                {
                    if (_currentPredictions.ContainsKey(predictedSign))
                    {
                        _currentPredictions[predictedSign]++;
                    }
                    else
                    {
                        _currentPredictions.Add(predictedSign, 1);
                    }
                }

                _udpServer.ReceivedData = null;
            }
        }
    }

    private void CalculateScore(string _expectedPrediction)
    {
        string feedback;

        // Using a default accuracy of 50% to help the user's scoring when the model makes incorrect predictions
        // Doing the correct signs still makes the player achieve higher scores compared to doing incorrect signs
        int totalPredictions = 10;
        int correctPredictions = 5;
        
        if (_currentPredictions.Keys.Count != 0)
        {            
            foreach (string predictionKey in _currentPredictions.Keys)
            {
                if (predictionKey == _expectedPrediction)
                {
                    correctPredictions += _currentPredictions[predictionKey];
                }

                totalPredictions += _currentPredictions[predictionKey];
            }

            float accuracy = (float)correctPredictions / totalPredictions; 

            if (accuracy >= 0.55)
            {
                _totalScore += 1000;
                feedback = "PERFECT";
            }
            else if (accuracy >= 0.35)
            {
                _totalScore += 500;
                feedback = "GREAT";
            }
            else if (accuracy >= 0.25)
            {
                _totalScore += 300;
                feedback = "GOOD";
            }
            else if (accuracy >= 0.15)
            {
                _totalScore += 100;
                feedback = "OK";
            }
            else
            {
                feedback = "MISS";
            }
        }
        else
        {
            feedback = "MISS";
        }

        feedbackManagerInstance.ShowFeedback(feedback); // Update the TextMeshPro text with the feedback
        feedbackGhostManagerInstance.ShowFeedback(feedback); // Update the TextMeshPro text with the feedback
    }

    private void StopGame()
    {
        _udpServer.SendData("STOP PREDICTIONS");
        _runningPredictions = false;

        // Reset prediction values
        _expectedFrame = null;
        _expectedPrediction = null;
        _currentPredictions.Clear();
        
        _songInformation.Clear();
        _expectedPredictions.Clear();
        _udpServer.ReceivedData = null;
        
        feedbackManagerInstance.ShowFeedback("");
        feedbackGhostManagerInstance.ShowFeedback("");

        // Switch to the GameOver scene
        SceneManager.LoadScene("GameOver");
    }

    void OnDestroy()
    {
        // Close the connection between servers
        _pythonClient.Kill();
    }
}