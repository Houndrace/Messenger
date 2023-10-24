using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using Messenger.Client.Models;

namespace Messenger.Client.Services;

public class ClientService : IClientService
{
    private readonly XmlSerializer _serializer = new(typeof(Message));
    private Socket? _connectionSocket;
    private NetworkStream? _networkStream;
    private IPAddress? _serverIpAddress; // IP адрес сервера
    private int _serverPort;

    public event IClientService.MessageRecieveHandler? MessageRecieved;

    public void SendMessage(Message message)
    {
        if (_networkStream is null)
            throw new NullReferenceException("NetworkStream is null");
        
        var memoryStream = new MemoryStream();
        _serializer.Serialize(memoryStream, message);

        var xmlData = memoryStream.ToArray();

        _networkStream.Write(xmlData);
    }

    public void ConnectToServer(IPAddress ipAddress, int port)
    {
        _serverIpAddress = ipAddress;
        _serverPort = port;
        _connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var serverEndPoint = new IPEndPoint(_serverIpAddress, _serverPort);

        _connectionSocket.Connect(serverEndPoint);

        _networkStream = new NetworkStream(_connectionSocket);

        StartServerReading();
    }

    private async void StartServerReading()
    {
        if (_networkStream is null)
            throw new NullReferenceException("NetworkStream is null");
        
        while (true)
        {
            var buffer = new byte[1024];
            var bytesRead = await _networkStream.ReadAsync(buffer);
            HandleRecievedMessage(buffer, bytesRead);
        }
    }

    private void HandleRecievedMessage(byte[] buffer, int bytesRead)
    {
        var memoryStream = new MemoryStream(buffer, 0, bytesRead);
        var message = (Message?)_serializer.Deserialize(memoryStream);

        if (message is null) return;

        MessageRecieved?.Invoke(message);
    }
}