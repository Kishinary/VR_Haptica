using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System;

public class TCPGravityStreamer : MonoBehaviour
{
    private Rigidbody rb;
    private TcpClient tcpClient;
    private NetworkStream stream;
    private const string ipAddress = "127.0.0.1";
    private const int port = 12345;
    public bool isCollision = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(ipAddress, port);
                stream = tcpClient.GetStream();
                Debug.Log("Connected to TCP server");
            }
            catch (Exception e)
            {
                Debug.LogError($"TCP Connection Error: {e.Message}");
            }
        }
        else
        {
            Debug.LogError("No Rigidbody component found on the cube!");
        }
    }

    void Update()
    {
        if (rb != null && stream != null && tcpClient.Connected)
        {
            float gravity = rb.mass * Physics.gravity.magnitude;
            SendTCPData(gravity, isCollision);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        isCollision = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isCollision = false;
    }

    void SendTCPData(float gravity, bool collisionState)
    {
        try
        {
            string data = $"{gravity},{collisionState}\n";
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            stream.Write(dataBytes, 0, dataBytes.Length);
            Debug.Log($"Sent: {data}");
        }
        catch (Exception e)
        {
            Debug.LogError($"TCP Send Error: {e.Message}");
        }
    }

    void OnApplicationQuit()
    {
        stream?.Close();
        tcpClient?.Close();
    }
}