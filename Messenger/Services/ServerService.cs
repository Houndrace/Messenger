﻿using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Messenger.Models;

namespace Messenger.Services;

public class ServerService : IServerService
{
    private const int MaxServerUser = 2;
    private const int _serverPort = 12345;
    private readonly IPAddress _ipAddress = IPAddress.Any;

    private readonly Mutex _mutex = new();
    private readonly XmlSerializer _serializer = new(typeof(Message));
    private readonly ICollection<Socket> _connectedSocketCollection = new List<Socket>();

    public event IServerService.MessageRecieveHandler? MessageRecieved;

    public async Task StartServer()
    {
        var endPoint = new IPEndPoint(_ipAddress, _serverPort);
        var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        var connectedUser = 0;
        listener.Bind(endPoint);
        listener.Listen(MaxServerUser);

        while (true)
        {
            var clientSocket = await listener.AcceptAsync();
            _connectedSocketCollection.Add(clientSocket);

            _ = StartClientReading(clientSocket);

            connectedUser++;

            if (connectedUser == MaxServerUser)
            {
                listener.Close();
                break;
            }
        }
    }

    private async Task StartClientReading(Socket clientSocket)
    {
        var stream = new NetworkStream(clientSocket);

        while (true)
        {
            var buffer = new byte[1024];
            var bytesRead = await stream.ReadAsync(buffer);

            var memoryStream = new MemoryStream(buffer, 0, bytesRead);
            var message = (Message?)_serializer.Deserialize(memoryStream);

            if (message is null) continue;

            message.MessageType = MessageType.Recieved;

            MessageRecieved?.Invoke(message);

            await SyncUserMessages(clientSocket, message);
        }
    }

    private async Task SyncUserMessages(Socket senderSocket, Message message)
    {
        foreach (var socket in _connectedSocketCollection)
        {
            if (socket == senderSocket) continue;

            var memoryStream = new MemoryStream();
            _serializer.Serialize(memoryStream, message);

            var xmlData = memoryStream.ToArray();

            var stream = new NetworkStream(socket);
            await stream.WriteAsync(xmlData);
        }
    }
}