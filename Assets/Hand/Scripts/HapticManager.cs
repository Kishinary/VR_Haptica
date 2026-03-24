using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class HapticManager : MonoBehaviour
{
    private Rigidbody rb;
    private TcpClient tcpClient;
    private NetworkStream stream;
    private const string ipAddress = "127.0.0.1";
    private const int port = 12345;
    public bool isCollision = false;
    private RunTIme_Debuger debuger;

    UdpClient udpClient;

    void Start()
    {
        udpClient = new UdpClient();
        debuger = FindFirstObjectByType<RunTIme_Debuger>();
    }

    public void SendHapticSignal(string fingerID, int intensity)
    {
        string message = fingerID + "," + intensity;
        byte[] data = Encoding.ASCII.GetBytes(message);
        udpClient.Send(data, data.Length, ipAddress, port);
        Debug.Log(message);
        debuger.SpawnDebug(message);
    }

    void OnApplicationQuit()
    {
        udpClient.Close();
    }
}