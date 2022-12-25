using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Homework3
{
    public class HW3Server : MonoBehaviour
    {
        #region Fields

        private const int MAX_CONNECTIONS = 10;

        public Button StartServerButton;
        public Button StopServerButton;
        public Text ServerStateText;
        public Text ServerLogText;

        private List<int> _connectionIDs = new List<int>();

        private int _reliableChannel;
        private int _hostID;
        private int _port = 3839;
        private bool _isActive = false;

        #endregion

        #region Properties

        public string Address { get => "127.0.0.1"; }
        public int Port { get => _port; }
        public int MaxConnections { get => MAX_CONNECTIONS; }

        #endregion

        #region UnityMethods

        private void Start()
        {
            StartServerButton.onClick.AddListener(() =>
            { 
                StartServer();
                StartServerButton.interactable = false;
                StopServerButton.interactable = true;
            });
            StopServerButton.onClick.AddListener(() =>
            {
                StopServer();
                StartServerButton.interactable = true;
                StopServerButton.interactable = false;
            });
        }

        private void Update()
        {
            if (!_isActive)
                return;

            RecieveData();
        }

        #endregion

        #region Methods

        private void StartServer()
        {
            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            _reliableChannel = config.AddChannel(QosType.Reliable);
            HostTopology topology = new HostTopology(config, MAX_CONNECTIONS);
            _hostID = NetworkTransport.AddHost(topology, _port);
            ServerStateText.text = "Active. Port:" + _port;
            _isActive = true;
        }

        private void StopServer()
        {
            NetworkTransport.RemoveHost(_hostID);
            NetworkTransport.Shutdown();
            ServerStateText.text = "Inactive";
            _isActive = false;
        }

        private void RecieveData()
        {
            var recBuffer = new byte[1024];
            var bufferSize = recBuffer.Length;

            var recievedData = NetworkTransport.Receive(
                out var recHostID,
                out var connectionID,
                out var channelID,
                recBuffer,
                bufferSize,
                out var dataSize,
                out var error
                );

            ServerLogText.text = $"Recieved data type: {recievedData}";

            while (recievedData != NetworkEventType.Nothing)
            {
                switch (recievedData)
                {
                    case NetworkEventType.Nothing:
                        Debug.Log("Nothing");
                        break;
                    case NetworkEventType.DataEvent:
                        Debug.Log("DataEvent");
                        break;
                    case NetworkEventType.ConnectEvent:
                        Debug.Log("ConnectEvent");
                        _connectionIDs.Add(connectionID);
                        ServerLogText.text = $"User {connectionID} connected";
                        break;
                    case NetworkEventType.DisconnectEvent:
                        Debug.Log("DisconnectEvent");
                        _connectionIDs.Remove(connectionID);
                        ServerLogText.text = $"User {connectionID} disconnected";
                        break;
                    case NetworkEventType.BroadcastEvent:
                        Debug.Log("BroadcastEvent");
                        break;
                }
            }
        }

        #endregion
    }
}