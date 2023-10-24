using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using Messenger.Console.Client;

namespace Messenger.Server;

public class Server
{
    private readonly ICollection<Socket> _connectedSocketCollection = new List<Socket>();
    private readonly IPAddress _ipAddress;
    private readonly int _maxServerUser;
    private readonly int _port;
    private readonly XmlSerializer _serializer = new(typeof(Message));


    public Server(IPAddress ipAddress, int port, int maxServerUser)
    {
        _ipAddress = ipAddress;
        _port = port;
        _maxServerUser = maxServerUser;
    }

    public async Task StartServer()
    {
        var endPoint = new IPEndPoint(_ipAddress, _port);
        var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        var connectedUser = 0;
        listener.Bind(endPoint);
        listener.Listen();

        while (true)
        {
            var clientSocket = await listener.AcceptAsync();
            _connectedSocketCollection.Add(clientSocket);

            _ = StartClientReading(clientSocket);

            connectedUser++;

            if (connectedUser == _maxServerUser)
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

            if (bytesRead == 0) return;

            var memoryStream = new MemoryStream(buffer, 0, bytesRead);
            var message = (Message?)_serializer.Deserialize(memoryStream);

            if (message is null) continue;

            SyncUsers(clientSocket, message);
        }
    }

    private void SyncUsers(Socket senderSocket, Message message)
    {
        foreach (var socket in _connectedSocketCollection)
        {
            if (socket == senderSocket || !socket.Connected) continue;

            var memoryStream = new MemoryStream();
            _serializer.Serialize(memoryStream, message);

            var xmlData = memoryStream.ToArray();

            var stream = new NetworkStream(socket);
            stream.WriteAsync(xmlData);
        }
    }
}