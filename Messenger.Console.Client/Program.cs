using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;
using Messenger.Console.Client;
using Messenger.Models;

var serverIp = "127.0.0.1"; // IP адрес сервера
var serverPort = 12345; // Порт сервера

try
{
    var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    var serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

    clientSocket.Connect(serverEndPoint);

    var message = new Message("fasd", "daun", MessageType.Sended);
    var serializer = new XmlSerializer(typeof(Message));
    var memoryStream = new MemoryStream();
    serializer.Serialize(memoryStream, message);
    var xmlData = memoryStream.ToArray();

    var stream = new NetworkStream(clientSocket);

    stream.Write(xmlData);
    Console.WriteLine("Отправлено: ");

    var buffer = new byte[1024];
    var bytesRead = clientSocket.Receive(buffer);
    var response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
    Console.WriteLine("Ответ сервера: " + response);

    clientSocket.Shutdown(SocketShutdown.Both);
    clientSocket.Close();
}
catch (Exception ex)
{
    Console.WriteLine("Ошибка: " + ex.Message);
}

Console.ReadKey();