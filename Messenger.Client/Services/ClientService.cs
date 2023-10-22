using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Xml.Serialization;
using Messenger.Client.Models;

namespace Messenger.Client.Services;

public class ClientService : IClientService
{
    private readonly XmlSerializer _serializer = new(typeof(Message));
    private Socket? _clientSocket;
    private IPAddress _serverIpAddress; // IP адрес сервера
    private int _serverPort;


    public bool? Connected => _clientSocket?.Connected;

    public event IClientService.MessageRecieveHandler? MessageRecieved;

    public void SendMessage(Message message)
    {
        var memoryStream = new MemoryStream();
        _serializer.Serialize(memoryStream, message);

        var xmlData = memoryStream.ToArray();

        var stream = new NetworkStream(_clientSocket);
        stream.Write(xmlData);
    }

    public void ConnectToServer(IPAddress ipAddress, int port)
    {
        _serverIpAddress = ipAddress;
        _serverPort = port;

        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var serverEndPoint = new IPEndPoint(_serverIpAddress, _serverPort);

        _clientSocket.Connect(serverEndPoint);

        StartServerReading();
    }

    public void StartServerReading()
    {
        var readThread = new Thread(() =>
        {
            while (true)
            {
                var buffer = new byte[1024];
                var bytesRead = _clientSocket.Receive(buffer);

                var memoryStream = new MemoryStream(buffer, 0, bytesRead);
                var message = (Message?)_serializer.Deserialize(memoryStream);

                if (message is null) continue;

                message.MessageType = MessageType.Recieved;

                Application.Current.Dispatcher.Invoke(() => { MessageRecieved?.Invoke(message); });
            }
        });
        readThread.Start();
    }
}