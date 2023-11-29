using UnityEngine;
using System;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

using Debug = System.Diagnostics.Debug;
public class Player : MonoBehaviour
{
    public SocketIOUnity socket;

    void Start(){
        socket = new SocketIOUnity("http://localhost:1032", new SocketIOOptions
        {
            Query = new Dictionary<string, string>
                {
                    {"token", "UNITY" }
                }
            ,
            EIO = 4
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        socket.JsonSerializer = new NewtonsoftJsonSerializer();
        socket.OnConnected += (sender, e) =>
        {
            Debug.Print("socket.OnConnected");
        };
        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Print("disconnect: " + e);
        };
        socket.OnReconnectAttempt += (sender, e) =>
        {
            Debug.Print($"{DateTime.Now} Reconnecting: attempt = {e}");
        };

    }
    void Update()
    {
        
    }
}