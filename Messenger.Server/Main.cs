using System.Net;
using Messenger.Server;

const int maxServerUser = 20;
var serverIpAddress = IPAddress.Any;
const int serverPort = 12345;

var server = new Server(serverIpAddress, serverPort, maxServerUser);

await server.StartServer();

while (true) Console.ReadKey();