using UnityEngine;
using System.Text;
using System.Net.Sockets;
using System;

public class UDPFingerCollisionWithGravity : MonoBehaviour
{
    private UdpClient udpClient;

    [Header("UDP Settings")]
    public string ipAddress = "127.0.0.1";
    public int port = 12345;

    [Header("Send Frequency (Hz)")]
    [Range(1f, 60f)] public float sendFrequency = 10f;

    private float sendTimer = 0f;

    [Header("External Object for Mass")]
    public GameObject externalObject; // External GameObject to get mass from

    void Start()
    {
        udpClient = new UdpClient();
        Debug.Log($"✅ UDPFingerCollisionWithGravity started. Sending at {sendFrequency} Hz to {ipAddress}:{port}");
    }

    // OnEnable, OnDisable, OnHandCollisionStateChanged are removed as per simplified logic.

    void Update()
    {
        if (udpClient == null) return;

        sendTimer += Time.deltaTime;
        float sendInterval = 1f / sendFrequency;

        if (sendTimer >= sendInterval)
        {
            float mass = (externalObject != null && externalObject.GetComponent<Rigidbody>() != null)
                ? externalObject.GetComponent<Rigidbody>().mass
                : 1.0f; // Default mass = 1 if no externalObject or Rigidbody

            float gravity = mass * Physics.gravity.magnitude;

            // Read collision states from HandCollisionStateManager Instance
            //int rightThumbState = HandCollisionStateManager.Instance.RightThumbCollidingState;
            //int rightIndexState = HandCollisionStateManager.Instance.RightIndexFingerCollidingState;

            // Get the continuous raw finger distance from RawFingerDistanceMonitor
            //float rawFingerDistance = RawFingerDistanceMonitor.CurrentRawFingerTipDistance;

            // Send 4 data values via UDP: thumb state, index state, gravity, raw finger distance
            //SendUDPData(rightThumbState, rightIndexState, gravity, rawFingerDistance);
            //sendTimer = 0f;
        }
    }

    // Method to send data via UDP (now takes 4 parameters)
    void SendUDPData(int rightThumb, int rightIndex, float gravity, float rawFingerDistance)
    {
        try
        {
            // Data format: "ThumbState,IndexState,Gravity,RawDistance" (e.g., "0,1,9.81,0.0356")
            string data = $"{rightThumb},{rightIndex},{gravity:F2},{rawFingerDistance:F4}";
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            udpClient.Send(dataBytes, dataBytes.Length, ipAddress, port);
            Debug.Log($"Sent via UDP: {data} at {Time.time:F2}s");
        }
        catch (Exception e)
        {
            Debug.LogError($"UDP Send Error: {e.Message}");
        }
    }

    void OnApplicationQuit()
    {
        udpClient?.Close();
        Debug.Log("UDP Client closed.");
    }
}