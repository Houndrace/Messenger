using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Server
{
    private const int maxClients = 2;
    private static readonly List<Socket> clients = new();

    private static async Task Main(string[] args)
    {
        var serverIP = IPAddress.Parse("127.0.0.1");
        var serverPort = 12345;

        while (true)
        {
            var listener = new TcpListener(serverIP, serverPort);
            listener.Start();
            Console.WriteLine("Сервер запущен.");

            while (clients.Count < maxClients)
            {
                var clientSocket = await listener.AcceptSocketAsync();
                clients.Add(clientSocket);

                _ = HandleClientAsync(clientSocket);
            }

            listener.Stop();
        }
    }

    private static async Task HandleClientAsync(Socket clientSocket)
    {
        using var clientStream = new NetworkStream(clientSocket);
        var buffer = new byte[1024];

        while (true)
        {
            var bytesRead = await clientStream.ReadAsync(buffer, 0, buffer.Length);

            if (bytesRead == 0)
            {
                // Клиент отключился, удаляем его из списка и завершаем обработку
                clients.Remove(clientSocket);
                clientSocket.Close();
                return;
            }

            var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Получено сообщение от клиента: {message}");

            // Отправляем сообщение другим клиентам
            foreach (var otherClient in clients)
                if (otherClient != clientSocket)
                {
                    using var otherClientStream = new NetworkStream(otherClient);
                    var responseBytes = Encoding.UTF8.GetBytes(message);
                    await otherClientStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                }
        }
    }
}