using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;
using Messenger.Console.Client;

int port = 12345;
IPAddress ipAddress = IPAddress.Any;
IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

try
{
    listener.Bind(endPoint);
    listener.Listen(5); // Ожидаем до 5 клиентов

    Console.WriteLine("Сервер запущен. Ожидание подключений...");

    while (true)
    {
        Socket clientSocket = listener.Accept();
        
        NetworkStream stream = new NetworkStream(clientSocket);
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);

        MemoryStream memoryStream = new MemoryStream(buffer, 0, bytesRead);
        XmlSerializer serializer = new XmlSerializer(typeof(Message));
        Message message = (Message)serializer.Deserialize(memoryStream);
        Console.WriteLine("Получено сообщение: " + message.Content);

        // Отправляем ответ клиенту
        byte[] responseBuffer = Encoding.UTF8.GetBytes("Сообщение получено");
        clientSocket.Send(responseBuffer);

        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }
}
catch (Exception ex)
{
    Console.WriteLine("Ошибка: " + ex.Message);
}
finally
{
    listener.Close();
}

Console.ReadKey();