using UnityEngine;
using UnityEngine.Video;

using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine.SceneManagement;

// NOTES: 
// Currently, data is bloated because its grabbing recieved data 30-60 times per second.
// The Update() method runs once per frame. If the prediction system generates predictions at a high frequency (let's say 60 times a second, roughly matching the frame rate), that might be why we see dozens of duplicate predictions within a single frame.

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

    // Instead of a List<string> to store _currentPredictions, lets try a HashSet<string>. A HashSet will not add duplicate entries. Each time you try to add a duplicate, it just ignores the addition.
    // private List<string> _currentPredictions;
    // private HashSet<string> _currentPredictions;
    private Dictionary<string, int> _currentPredictions;


    private int _totalScore = 0;

    public void Start()
    {
        _udpServer = UDP_Server.Instance;
        _runningPredictions = false;
        
        _songInformation = new Dictionary<string, string>();
        _expectedPredictions = new Queue<(int, string)>();

        _expectedFrame = null;
        _expectedPrediction = null;
        _currentPredictions = new Dictionary<string, int>();

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

            // Once the received data has been recorded for a frame in the Update() method, reset the _udpServer.ReceivedData so it doesn't keep reading the same data on subsequent frames.
 
            // string predictedSign = _udpServer.ReceivedData;

            // if (predictedSign != null)
            // {
            //     _currentPredictions.Add(_udpServer.ReceivedData);
            // }

            string predictedSign = _udpServer.ReceivedData;

            // To filter out "NO DETECTIONS" from the predictions that the player makes, modify the hashset
            // if (predictedSign != null && predictedSign != "NO DETECTIONS")
            // {
            //     _currentPredictions.Add(predictedSign);
            // }

            // if (predictedSign != null)
            // {
            //     _currentPredictions.Add(predictedSign);
            //     _udpServer.ReceivedData = null;
            // }

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

    private string CalculateScore(string _expectedPrediction)
    {
        int correctPredictions = 0; // Count of how many predictions were correct

        // Loop through each prediction made by the player
        // for (int i = 0; i < _currentPredictions.Count; i++)
        // {
        //     // If the player's prediction matches the expected sign
        //     // if (_currentPredictions[i] == _expectedPrediction)
        //     // {
        //     //     // Increase the count of correct predictions
        //     //     correctPredictions++;
        //     // }
        //     if (_currentPredictions.Contains(_expectedPrediction))
        //     {
        //         correctPredictions++;
        //     }

        // }

        if (_currentPredictions.ContainsKey(_expectedPrediction))
        {
            correctPredictions = 1;
        }

        float accuracy = (float)correctPredictions / _currentPredictions.Count; 

        UnityEngine.Debug.Log($"Expected prediction: {_expectedPrediction}");
        UnityEngine.Debug.Log(string.Join(", ", _currentPredictions));
        UnityEngine.Debug.Log($"Accuracy: {accuracy * 100}%");

        if (accuracy >= 0.9) { _totalScore += 100; return "PERFECT"; }
        if (accuracy >= 0.8) { _totalScore += 80; return "GREAT"; }
        if (accuracy >= 0.6) { _totalScore += 60; return "GOOD"; }
        if (accuracy >= 0.4) { _totalScore += 40; return "OK"; }
        return "MISS"; // In this setup, when a "MISS" happens, the _totalScore remains the same as it was before. The player neither gains nor loses points.
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