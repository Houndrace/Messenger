using System.Net;
using Messenger.Client.Models;

namespace Messenger.Client.Services;

public interface IClientService
{
    delegate void ConnectHandler();

    delegate void DisconnectHandler();

    delegate void MessageRecieveHandler(Message message);

    bool? IsConnected { get; }

    event MessageRecieveHandler MessageRecieved;

    event ConnectHandler Connected;
    event DisconnectHandler Disconnected;

    void SendMessage(Message message);

    void ConnectToServer(IPAddress ipAddress, int port);
}