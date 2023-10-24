using System.Threading.Tasks;
using Messenger.Models;

namespace Messenger.Services;

public interface IServerService
{ 
    delegate void MessageRecieveHandler(Message message);
    
    event MessageRecieveHandler MessageRecieved;
    Task StartServer();
}