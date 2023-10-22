using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Xml.Serialization;
using Messenger.Models;

namespace Messenger.Services;

public class ServerService : IServerService
{
    private const int MaxServerUser = 2;
    private readonly IPAddress _ipAddress = IPAddress.Any;
    private const int _serverPort = 12345;

    private readonly Mutex _mutex = new();
    private readonly XmlSerializer _serializer = new(typeof(Message));
    private Socket? _clientSocket;
    
    public event IServerService.MessageRecieveHandler? MessageRecieved;

    public void StartServer()
    {
        var startServerThread = new Thread(() =>
        {
            var endPoint = new IPEndPoint(_ipAddress, _serverPort);
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            int connectedUser = 0;
            listener.Bind(endPoint);

            while (true)
            {
                listener.Listen(MaxServerUser);
                _clientSocket = listener.Accept();

                new Thread(StartReadingSocket).Start();
                
                connectedUser++;

                if (connectedUser == MaxServerUser)
                {
                    listener.Close();
                    break;
                }
            }
        });
        startServerThread.Start();
    }

    public void SendResponse(Message message)
    {
        throw new NotImplementedException();
    }

    private void StartReadingSocket()
    {
        var stream = new NetworkStream(_clientSocket);

        var localSocket = _clientSocket;
        localSocket.
        
        while (true)
        {
            var buffer = new byte[1024];
            var bytesRead = stream.Read(buffer);

            var memoryStream = new MemoryStream(buffer, 0, bytesRead);
            var message = (Message?)_serializer.Deserialize(memoryStream);

            if (message is null) continue;

            message.MessageType = MessageType.Recieved;
                
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageRecieved?.Invoke(message);
            });
        }
    }
}