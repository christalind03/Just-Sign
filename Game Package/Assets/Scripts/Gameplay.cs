using UnityEngine;
using UnityEngine.Video;

using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Gameplay : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer _videoPlayer;

    private UDP_Server _udpServer;
    private bool _runningPredictions;

    private Dictionary<string, string> _songInformation;
    private Queue<(int, string)> _expectedPredictions;

    private int? expectedTime = null;
    private string expectedPrediction = null;
    private List<string> currentPredictions = new List<string>();

    public void Start()
    {
        _udpServer = UDP_Server.Instance;
        _runningPredictions = false;
        
        _songInformation = new Dictionary<string, string>();
        _expectedPredictions = new Queue<(int, string)>();

        string executableDirectory = Directory.GetCurrentDirectory() + "/Assets/Scripts/ASL Recognition/Executable File/ASL Recognition Script.exe";
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
        _udpServer.SendData("RUN PREDICTIONS");
        _runningPredictions = true;
    }

    public void Update()
    {
        if (_runningPredictions)
        {
            if (expectedTime == null)
            {
                try
                {
                    var expectedInformation = _expectedPredictions.Dequeue();

                    expectedTime = expectedInformation.Item1;
                    expectedPrediction = expectedInformation.Item2;
                }
                catch
                {
                    StopGame();
                }
            }
            else if ((expectedTime != null) && (_videoPlayer.time == expectedTime))
            {
                CalculateScore(expectedPrediction);

                expectedTime = null;
                expectedPrediction = null;
                currentPredictions.Clear();
            }
 
            currentPredictions.Add(_udpServer.GetData());

        }
    }

    private string CalculateScore(string expectedPrediction)
    {
        int correctPredictions = 0; // Count of how many predictions were correct

        // Loop through each prediction made by the player
        for (int i = 0; i < currentPredictions.Count; i++)
        {
            // If the player's prediction matches the expected sign
            if (currentPredictions[i] == expectedPrediction)
            {
                // Increase the count of correct predictions
                correctPredictions++;
            }
        }

        // Calculate the percentage of predictions that were correct
        float accuracy = (float)correctPredictions / currentPredictions.Count; 

        // Log the calculated accuracy for debugging purposes
        UnityEngine.Debug.Log($"Expected prediction: {expectedPrediction}");
        UnityEngine.Debug.Log(string.Join(", ", currentPredictions));
        UnityEngine.Debug.Log($"Accuracy: {accuracy * 100}%");

        // Return the score based on accuracy
        if (accuracy >= 0.9) return "PERFECT";
        if (accuracy >= 0.8) return "GREAT";
        if (accuracy >= 0.6) return "GOOD";
        if (accuracy >= 0.4) return "OK";
        return "MISS";
    }

    public void StopGame()
    {
        _udpServer.SendData("STOP PREDICTIONS");
        _runningPredictions = false;
    }

    public void OnDestroy()
    {
        // No need to end the process we started using Process.Kill() or something similar as sending "TERMINATE" via UDP will work
        _udpServer.SendData("TERMINATE");
    }
}