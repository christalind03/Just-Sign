using UnityEngine;

using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;

public class UDP_Server
{
    // Creating a singleton class since we only want one UDP_Server
    private const int PORT = 50505;
    private static UDP_Server _instance = null;
    private static readonly object _lockObject = new object();

    private UdpClient _udpServer;
    private IPEndPoint _clientEndPoint;

    private Thread _receivingThread;
    private string _receivedData;

    private UDP_Server()
    {
        _receivingThread = new Thread(new ThreadStart(ReceiveData));

        _receivingThread.IsBackground = true;
        _receivingThread.Start();
    }

    public static UDP_Server Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new UDP_Server();
                    }
                }
            }

            return _instance;
        }
    }

    public string GetData()
    {
        return _receivedData;
    }

    private void ReceiveData()
    {
        _udpServer = new UdpClient(PORT);
        _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {
            try
            {
                byte[] dataByte = _udpServer.Receive(ref _clientEndPoint);
                _receivedData = Encoding.UTF8.GetString(dataByte);

                Debug.Log(_receivedData); 
            }
            catch (Exception networkError)
            {
                Debug.Log(networkError.ToString());
            }
        }
    }

    public void SendData(string data)
    {
        byte[] responseData = Encoding.ASCII.GetBytes(data);
        _udpServer.Send(responseData, responseData.Length, _clientEndPoint);
    }

    ~UDP_Server()
    {
        _udpServer.Close();
        _receivingThread.Abort();
    }
}