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

    public enum ScoreResult
    {
        PERFECT,
        GREAT,
        GOOD,
        OK,
        MISS
    }

    public ScoreResult CalculateScore(List<string> allPredictions)
    {
        // Count the number of correct predictions within the list and divide it by total predictions
        // Have "PERFECT", "GREAT", "GOOD", "OK", and "MISS" scores based off percentages. 

        // My notes and understanding

            // The _expectedPredictions queue contains all the signs the player is expected to make for the song
            // in the order they are to be performed. These are the "lyrics" for game

            // When CalculateScore is called, it's provided with allPredictions, a list of the signs the player actually made while the song was playing. 
            // The function then checks each sign in allPredictions against the signs in _expectedPredictions to count how many were correct. 
            // This count is stored in correctPredictions.

            // Accuracy = correctPredictions / total # of signs in song (ie size of allPredictions)

            // Based on the calculated accuracy:
                // 90% and above is graded "PERFECT".
                // 80% to 89% is graded "GREAT".
                // 60% to 79% is "GOOD".
                // 40% to 59% is "OK".
                // Anything below 40% is graded as "MISS".

        int correctPredictions = 0; // Count of how many predictions were correct

        // If there are no predictions or if there's a mismatch in number of predictions and expected predictions
        if (allPredictions.Count == 0 || allPredictions.Count != _expectedPredictions.Count)
        {
            UnityEngine.Debug.Log("Number of predictions does not match expected predictions.");
            return ScoreResult.MISS;
        }

        // Convert the queue of expected predictions to an array to facilitate indexed access
        // This avoids deforming the original queue during our calculation
        var expectedPredictionsArray = _expectedPredictions.ToArray();

        // Loop through each prediction made by the player
        for (int i = 0; i < allPredictions.Count; i++)
        {
            // Extract the expected sign at the current index from our array
            var (_, expectedSign) = expectedPredictionsArray[i];

            // If the player's prediction matches the expected sign
            if (allPredictions[i] == expectedSign)
            {
                // Increase the count of correct predictions
                correctPredictions++;
            }
        }

        // Calculate the percentage of predictions that were correct
        float accuracy = (float)correctPredictions / allPredictions.Count; 

        // Log the calculated accuracy for debugging purposes
        UnityEngine.Debug.Log($"Accuracy: {accuracy * 100}%");

        // Return the score based on accuracy
        if (accuracy >= 0.9) return ScoreResult.PERFECT;
        if (accuracy >= 0.8) return ScoreResult.GREAT;
        if (accuracy >= 0.6) return ScoreResult.GOOD;
        if (accuracy >= 0.4) return ScoreResult.OK;
        return ScoreResult.MISS; // Anything below 40% accuracy is a miss
    }



    public void StopGame()
    {
        _udpServer.SendData("STOP PREDICTIONS");
        _runningPredictions = false;
    }
}
