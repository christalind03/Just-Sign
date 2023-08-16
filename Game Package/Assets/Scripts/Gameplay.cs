using UnityEngine;
using UnityEngine.Video;

using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine.SceneManagement;


public class Gameplay : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer _videoPlayer;

    private UDP_Server _udpServer;
    private bool _runningPredictions;

    private Dictionary<string, string> _songInformation;
    private Queue<(int, string)> _expectedPredictions;

    private int? _expectedFrame;
    private string _expectedPrediction;
    private List<string> _currentPredictions;

    private int _totalScore = 0;

    public void Start()
    {
        _udpServer = UDP_Server.Instance;
        _runningPredictions = false;
        
        _songInformation = new Dictionary<string, string>();
        _expectedPredictions = new Queue<(int, string)>();

        _expectedFrame = null;
        _expectedPrediction = null;
        _currentPredictions = new List<string>();

        string executableDirectory = Directory.GetCurrentDirectory() + "/Assets/Scripts/ASL Recognition/Executable.exe";
        Process.Start(executableDirectory);
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
                    StopGame();
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
                _currentPredictions.Add(_udpServer.ReceivedData);
            }
        }
    }

    private string CalculateScore(string _expectedPrediction)
    {
        int correctPredictions = 0; // Count of how many predictions were correct

        // Loop through each prediction made by the player
        for (int i = 0; i < _currentPredictions.Count; i++)
        {
            // If the player's prediction matches the expected sign
            if (_currentPredictions[i] == _expectedPrediction)
            {
                // Increase the count of correct predictions
                correctPredictions++;
            }
        }

        // Calculate the percentage of predictions that were correct
        float accuracy = (float)correctPredictions / _currentPredictions.Count; 

        // Log the calculated accuracy for debugging purposes
        UnityEngine.Debug.Log($"Expected prediction: {_expectedPrediction}");
        UnityEngine.Debug.Log(string.Join(", ", _currentPredictions));
        UnityEngine.Debug.Log($"Accuracy: {accuracy * 100}%");

        // Return the score based on accuracy
        if (accuracy >= 0.9) { _totalScore += 100; return "PERFECT"; }
        if (accuracy >= 0.8) { _totalScore += 80; return "GREAT"; }
        if (accuracy >= 0.6) { _totalScore += 60; return "GOOD"; }
        if (accuracy >= 0.4) { _totalScore += 40; return "OK"; }
        return "MISS";
    }

    public int GetTotalScore()
    {
        return _totalScore;
    }

    public void StopGame()
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

        // Switch to the GameOver scene
        SceneManager.LoadScene("GameOver");
    }

    public void OnDestroy()
    {
        _udpServer.SendData("STOP PREDICTIONS");
        _runningPredictions = false;

        // No need to end the process we started using Process.Kill() or something similar as stopping the game and sending "TERMINATE" via UDP will work
        _udpServer.SendData("TERMINATE");
    }
}