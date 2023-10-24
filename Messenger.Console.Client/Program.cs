using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Client
{
    private static async Task Main(string[] args)
    {
        var serverIP = IPAddress.Parse("127.0.0.1");
        var serverPort = 12345;

        var client = new TcpClient();
        await client.ConnectAsync(serverIP, serverPort);
        Console.WriteLine("Подключено к серверу.");

        using var clientStream = client.GetStream();
        var buffer = new byte[1024];

        while (true)
        {
            Console.Write("Введите сообщение: ");
            var message = Console.ReadLine();
            var messageBytes = Encoding.UTF8.GetBytes(message);
            await clientStream.WriteAsync(messageBytes, 0, messageBytes.Length);

            var bytesRead = await clientStream.ReadAsync(buffer, 0, buffer.Length);
            var response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Ответ от сервера: {response}");
        }
    }
}