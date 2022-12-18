using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class practice : MonoBehaviour
{
    #region Fields

    [System.Obsolete]
    private GlobalConfig gConfig = new GlobalConfig();
    [System.Obsolete]
    private ConnectionConfig config = new ConnectionConfig();
    [System.Obsolete]
    private HostTopology topology;

    private int myReliableChannelId;
    private int myUnreliableChannelId;
    private int connectionId;
    private int hostId;

    #endregion

    #region UnityMethods

    [System.Obsolete]
    void Start()
    {
        //gConfig.MaxPacketSize = 500;
        NetworkTransport.Init();// gConfig);

        config.AddChannel(QosType.Reliable);
        config.AddChannel(QosType.Unreliable);

        topology = new HostTopology(config, 10);

        hostId = NetworkTransport.AddHost(topology, 8888);

        connectionId = NetworkTransport.Connect(hostId, "127.0.0.1", 8888, 0, out var error);
        if ((NetworkError)error != NetworkError.Ok)
            Debug.Log((NetworkError)error);

        //NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        //NetworkTransport.ReceiveFromHost(recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

        byte[] buffer = new byte[1] { default };
        int bufferLength = buffer.Length;

        NetworkTransport.Send(hostId, connectionId, myReliableChannelId, buffer, bufferLength, out var error2);
        if ((NetworkError)error2 != NetworkError.Ok)
            Debug.Log((NetworkError)error);

        NetworkTransport.Disconnect(hostId, connectionId, out var error3);
        if ((NetworkError)error3 != NetworkError.Ok)
            Debug.Log((NetworkError)error);
    }

    void Update()
    {
        
    }

    #endregion
}
