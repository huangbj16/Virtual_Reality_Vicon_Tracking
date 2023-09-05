using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class TcpSender : MonoBehaviour
{

    public String Host = "localhost";
    public Int32 Port = 9051;

    TcpClient client = null;
    NetworkStream stream = null;
    StreamWriter writer = null;
    public bool isConnected = false;
    private bool isActiveState = true;

    // Start is called before the first frame update
    void Start()
    {
        client = new TcpClient();
        if (SetupSocket())
        {
            isConnected = true;
            Debug.Log("socket is set up");
            Debug.Log("buffer size length = "+client.SendBufferSize);
        }
        else
        {
            isConnected = false;
            Debug.Log("socket setup failed");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetActiveState(bool activeState)
    {
        isActiveState = activeState;
    }

    public void SendData(String data)
    {
        if (isActiveState)
        {
            Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(data);
            stream.Write(sendBytes, 0, sendBytes.Length);
            stream.Flush();
            Debug.Log("socket is sent");
        }
        else
        {
            Debug.Log("no socket is sent");
        }
    }

    public bool SetupSocket()
    {
        try
        {
            client.Connect(Host, Port);
            client.SendBufferSize = 512;
            client.ReceiveBufferSize = 512;
            stream = client.GetStream();
            //writer = new StreamWriter(stream);
            //Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes("1");
            //client.GetStream().Write(sendBytes, 0, sendBytes.Length);
            //Debug.Log("socket is sent");
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e);
            return false;
        }
    }

    private void OnApplicationQuit()
    {
        if (client != null && client.Connected)
            client.Close();
    }

}
