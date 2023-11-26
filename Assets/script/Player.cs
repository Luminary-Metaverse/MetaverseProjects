using UnityEngine;
using System;
using System.Collections.Generic;
using SocketIOClient.Newtonsoft.Json;

public class Player : MonoBehaviour
{
    private SocketIOUnity socket;
    private string playerId;

    // Prefab untuk merepresentasikan tangan pemain lain
    public GameObject otherUserHandPrefab;

    // Pengaturan gravitasi
    public float gravityStrength = 9.81f;
    public Transform gravityCenter;

    // Objek pemain
    public GameObject playerObjectPrefab;

    // Dictionary untuk menyimpan objek pemain lain
    private Dictionary<string, GameObject> otherPlayers = new Dictionary<string, GameObject>();
    // Dictionary untuk menyimpan tangan pemain lain
    private Dictionary<string, GameObject> otherPlayerHands = new Dictionary<string, GameObject>();

    void Start()
    {
        socket = new SocketIOUnity("http://localhost:1032");

        socket.On("updateGravity", (SocketIOResponse response) =>
        {
            UpdateGravity(response);
        });

        socket.On("updatePlayerMovement", (SocketIOResponse response) =>
        {
            UpdateOtherPlayerMovement(response);
        });

        socket.On("updateLeftHandMovement", (SocketIOResponse response) =>
        {
            UpdateLeftHandMovement(response);
        });

        socket.On("updateRightHandMovement", (SocketIOResponse response) =>
        {
            UpdateRightHandMovement(response);
        });

        socket.On("setPlayerId", (SocketIOResponse response) =>
        {
            SetPlayerId(response);
        });

        socket.On("updateOtherPlayerDisconnected", (SocketIOResponse response) =>
        {
            UpdateOtherPlayerDisconnected(response);
        });

        // Terhubung ke server
        socket.Connect();

        // Mengirimkan event playerConnected
        socket.Emit("playerConnected");
    }

    void Update()
    {
        // Mengecek input pemain lokal untuk pergerakan
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Menghitung gaya gravitasi
        Vector3 gravityDirection = (gravityCenter.position - transform.position).normalized;
        Vector3 gravityForce = gravityDirection * gravityStrength;

        // Mengaplikasikan gaya gravitasi pada pemain
        GetComponent<CharacterController>().Move(gravityForce * Time.deltaTime);

        // Emit player movement dan gravitasi ke server
        var playerData = new
        {
            playerId = playerId,
            x = transform.position.x,
            y = transform.position.y,
            z = transform.position.z
        };

        var gravityData = new
        {
            x = gravityCenter.position.x,
            y = gravityCenter.position.y,
            z = gravityCenter.position.z
        };

        socket.Emit("playerMovement", playerData);
        socket.Emit("gravityMovement", gravityData);

        // Emit pergerakan tangan dari kontroler VR (tangan kanan)
        Vector3 rightHandPosition = GetVRControllerPosition("right");
        var rightHandData = new
        {
            playerId = playerId,
            x = rightHandPosition.x,
            y = rightHandPosition.y,
            z = rightHandPosition.z
        };

        socket.Emit("updateRightHandMovement", rightHandData);

        // Emit pergerakan tangan dari kontroler VR (tangan kiri)
        Vector3 leftHandPosition = GetVRControllerPosition("left");
        var leftHandData = new
        {
            playerId = playerId,
            x = leftHandPosition.x,
            y = leftHandPosition.y,
            z = leftHandPosition.z
        };

        socket.Emit("updateLeftHandMovement", leftHandData);
    }

    private Vector3 GetVRControllerPosition(string controllerHand)
    {
        Vector3 handPosition = Vector3.zero;

        UnityEngine.XR.InputDevice device = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(controllerHand == "right" ? UnityEngine.XR.XRNode.RightHand : UnityEngine.XR.XRNode.LeftHand);
        device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out handPosition);

        return handPosition;
    }

    void UpdateGravity(SocketIOResponse response)
    {
        string jsonString = response.Json.GetField("args").ToString();
        GravityData gravityData = JsonConvert.DeserializeObject<GravityData>(jsonString);

        gravityCenter.position = new Vector3(gravityData.x, gravityData.y, gravityData.z);
    }

    void UpdateLeftHandMovement(SocketIOResponse response)
    {
        string jsonString = response.Json.GetField("args").ToString();
        HandMovementData handData = JsonConvert.DeserializeObject<HandMovementData>(jsonString);

        UpdateHandMovement("left", handData);
    }

    void UpdateRightHandMovement(SocketIOResponse response)
    {
        string jsonString = response.Json.GetField("args").ToString();
        HandMovementData handData = JsonConvert.DeserializeObject<HandMovementData>(jsonString);

        UpdateHandMovement("right", handData);
    }

    void UpdateOtherPlayerMovement(SocketIOResponse response)
    {
        string jsonString = response.Json.GetField("args").ToString();
        PlayerMovementData playerData = JsonConvert.DeserializeObject<PlayerMovementData>(jsonString);

        UpdatePlayerMovement(playerData);
    }

    void UpdateOtherPlayerDisconnected(SocketIOResponse response)
    {
        string jsonString = response.Json.GetField("args").ToString();
        string otherPlayerId = JsonConvert.DeserializeObject<string>(jsonString);

        // Memeriksa apakah objek pemain lain masih ada
        if (otherPlayers.ContainsKey(otherPlayerId))
        {
            // Menghancurkan objek pemain lain
            GameObject disconnectedPlayerObject = otherPlayers[otherPlayerId];
            Destroy(disconnectedPlayerObject);
            otherPlayers.Remove(otherPlayerId);
        }

        // Memeriksa apakah objek tangan pemain lain (kiri) masih ada
        if (otherPlayerHands.ContainsKey(otherPlayerId + "_left"))
        {
            // Menghancurkan objek tangan pemain lain (kiri)
            GameObject disconnectedPlayerHandLeft = otherPlayerHands[otherPlayerId + "_left"];
            Destroy(disconnectedPlayerHandLeft);
            otherPlayerHands.Remove(otherPlayerId + "_left");
        }

        // Memeriksa apakah objek tangan pemain lain (kanan) masih ada
        if (otherPlayerHands.ContainsKey(otherPlayerId + "_right"))
        {
            // Menghancurkan objek tangan pemain lain (kanan)
            GameObject disconnectedPlayerHandRight = otherPlayerHands[otherPlayerId + "_right"];
            Destroy(disconnectedPlayerHandRight);
            otherPlayerHands.Remove(otherPlayerId + "_right");
        }
    }

    void SetPlayerId(SocketIOResponse response)
    {
        string jsonString = response.Json.GetField("args").ToString();
        playerId = JsonConvert.DeserializeObject<string>(jsonString);
        Debug.Log("Menerima playerId dari server: " + playerId);
    }

    // ... (remaining code)

    [Serializable]
    public class GravityData
    {
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public class HandMovementData
    {
        public string playerId;
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public class PlayerMovementData
    {
        public string playerId;
        public float x;
        public float y;
        public float z;
    }
}