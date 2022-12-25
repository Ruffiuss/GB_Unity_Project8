using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

namespace Assets.Homework3
{
    public class Excercise1 : MonoBehaviour
    {
        #region Fields

        public HW3Server CurrentServer;
        public TMP_InputField InputField;
        public Button ConnectButton;
        public Button DisconnectButton;
        public Text PlayerLogText;
        public delegate void OnMessageReceive(object message);
        public event OnMessageReceive onMessageReceive;

        private bool _isConnected = false;
        private int _reliableChannel;
        private int _connectionID;
        private int _hostID;
        private int _port = 0;

        #endregion

        #region UnityMethods

        void Start()
        {
            ConnectButton.onClick.AddListener(() =>
            {
                if (ConnectServer())
                {
                    ConnectButton.interactable = false;
                    DisconnectButton.interactable = true;
                    PlayerLogText.text = default;
                    SendNameToServer(InputField.text);
                }
                else
                    PlayerLogText.text = "Can`t connect to server! Check log for details";

            });

            DisconnectButton.onClick.AddListener(()=>
            {
                DisconnectServer();
                ConnectButton.interactable = true;
                DisconnectButton.interactable = false;
            });
        }

        void Update()
        {
            if (!_isConnected)
                return;

            RecieveMessage();
        }

        #endregion

        #region Methods

        public bool ConnectServer()
        {
            NetworkTransport.Init();
            ConnectionConfig config = new ConnectionConfig();
            _reliableChannel = config.AddChannel(QosType.Reliable);
            HostTopology topology = new HostTopology(config, CurrentServer.MaxConnections);

            _hostID = NetworkTransport.AddHost(topology, _port);
            _connectionID = NetworkTransport.Connect(
                _hostID,
                CurrentServer.Address,
                CurrentServer.Port,
                0,
                out var error
                );

            if ((NetworkError)error == NetworkError.Ok)
            {
                _isConnected = true;
                return true;
            }
            else
            {
                PlayerLogText.text = ((NetworkError)error).ToString();
                _isConnected = false;
                return false;
            }
        }

        public void DisconnectServer()
        {
            if (!_isConnected)
                return;

            NetworkTransport.Disconnect(_hostID, _connectionID, out var error);
            Debug.Log(error);
            _isConnected = false;
        }

        private void RecieveMessage()
        {
            int recHostId;
            int connectionId;
            int channelId;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out
            channelId, recBuffer, bufferSize, out dataSize, out var error);
            while (recData != NetworkEventType.Nothing)
            {
                switch (recData)
                {
                    case NetworkEventType.Nothing:
                        break;
                    case NetworkEventType.ConnectEvent:
                        onMessageReceive?.Invoke($"You have been connected to server.");
                        PlayerLogText.text = $"You have been connected to server.";
                        break;
                    case NetworkEventType.DataEvent:
                        string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                        onMessageReceive?.Invoke(message);
                        PlayerLogText.text = message;
                        break;
                    case NetworkEventType.DisconnectEvent:
                        _isConnected = false;
                        ConnectButton.interactable = true;
                        DisconnectButton.interactable = false;
                        onMessageReceive?.Invoke($"You have been disconnected from server.");
                        PlayerLogText.text = $"You have been disconnected from server.";
                        break;
                    case NetworkEventType.BroadcastEvent:
                        break;
                }
                recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
                bufferSize, out dataSize, out error);
            }
        }

        private void SendNameToServer(string name)
        {
            if (!_isConnected)
            {
                PlayerLogText.text = "Unable send a message without connection to server";
                return;
            }
            byte[] buffer = Encoding.Unicode.GetBytes(name);
            NetworkTransport.Send(_hostID, _connectionID, _reliableChannel, buffer, name.Length * sizeof(char), out var error);
            if ((NetworkError)error != NetworkError.Ok)
                PlayerLogText.text = ((NetworkError)error).ToString();
        }

        #endregion
    }
}