using Messenger.Models;

namespace Messenger.Services;

public interface IServerService
{ 
    delegate void MessageRecieveHandler(Message message);
    
    event MessageRecieveHandler MessageRecieved;
    void StartServer();
    void SendResponse(Message message);
}