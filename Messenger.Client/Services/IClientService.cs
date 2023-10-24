using System.Net;
using Messenger.Client.Models;

namespace Messenger.Client.Services;

public interface IClientService
{
    delegate void MessageRecieveHandler(Message message);

    event MessageRecieveHandler MessageRecieved;

    void SendMessage(Message message);

    void ConnectToServer(IPAddress ipAddress, int port);
}