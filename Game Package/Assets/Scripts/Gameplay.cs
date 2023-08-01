using UnityEngine;

using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Gameplay : MonoBehaviour
{
    private UDP_Server _udpServer;
    private bool _runningPredictions;

    private Dictionary<string, string> _songInformation;
    private Queue<(int, string)> _expectedPredictions;

    void Start()
    {
        _udpServer = UDP_Server.Instance;
        _runningPredictions = false;
        
        _songInformation = new Dictionary<string, string>();
        _expectedPredictions = new Queue<(int, string)>();

        string executableDirectory = Directory.GetCurrentDirectory() + "/Assets/Scripts/ASL Recognition/Executable File/ASL Recognition Script.exe";
        Process.Start(executableDirectory);
    }

    public void LoadSong(string beatmapFilePath)
    {
        bool readingSongData = false;
        bool readingSignData = false;

        using (StreamReader fileReader = new StreamReader(beatmapFilePath))
        {
            string fileLine;

            while ((fileLine = fileReader.ReadLine()) != null)
            {
                fileLine = Regex.Replace(fileLine, @"\t|\n|\r", "");

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

    void Update()
    {
        if (_runningPredictions)
        {
            // Check if the expected predictions queue is empty
                // If not, pop the current item off and save in a variable
                // If it is, stop predictions

            // Keep track of all the predictions made so far with _udpServer.GetData() and a list
            // Once the current VideoPlayer time (https://docs.unity3d.com/560/Documentation/ScriptReference/Video.VideoPlayer-time.html) matches the time in the popped queue variable, calculate the score
            
            // CalculateScore();
        }
    }

    public void CalculateScore(List<string> allPredictions)
    {
        // Count the number of correct predictions within the list and divide it by total predictions
        // Have "PERFECT", "GREAT", "GOOD", "OK", and "MISS" scores based off percentages
        
        // UnityEngine.Debug.Log("Calculating score...");
    }

    public void StopGame()
    {
        _udpServer.SendData("STOP PREDICTIONS");
        _runningPredictions = false;
    }
}
