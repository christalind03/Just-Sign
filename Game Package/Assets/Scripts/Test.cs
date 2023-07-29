using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;

public class Test : MonoBehaviour
{
    private UDP_Server _udpServer;

    [SerializeField]
    private Toggle _runPredictions;

    [SerializeField]
    private Toggle _stopPredictions;

    [SerializeField]
    private Toggle _closeProgram;

    // Start is called before the first frame update
    void Start()
    {
        _udpServer = UDP_Server.Instance;
        RunPython();
        
        _runPredictions.onValueChanged.AddListener(delegate {UpdatePredictions(_runPredictions);});
        _stopPredictions.onValueChanged.AddListener(delegate {StopPredictions(_stopPredictions);});
        _closeProgram.onValueChanged.AddListener(delegate {UpdateProgram(_closeProgram);});
    }

    void RunPython()
    {
        string executableDirectory  = Directory.GetCurrentDirectory() + "/Assets/Scripts/ASL Recognition/Executable File/ASL Recognition Script.exe";
        Process.Start(executableDirectory);
    }

    void UpdatePredictions(Toggle change)
    {
        UnityEngine.Debug.Log("Predictions is now " + _runPredictions.isOn);
        _udpServer.SendData("RUN PREDICTIONS");
    }

    void StopPredictions(Toggle change)
    {
        UnityEngine.Debug.Log("Predictions is now " + _runPredictions.isOn);
        _udpServer.SendData("STOP PREDICTIONS");
    }

    void UpdateProgram(Toggle change)
    {
        UnityEngine.Debug.Log("Program is now " + _closeProgram.isOn);
        _udpServer.SendData("TERMINATE");
    }
}
