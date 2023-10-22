using Messenger.Models;

namespace Messenger.Console.Client;

[Serializable]
public class Message : IMessage
{
    public Message(string sender, string content, MessageType messageType)
    {
        Sender = sender;
        Content = content;
        SendDateTime = DateTime.Now;
        MessageType = messageType;
    }

    public Message()
    {
    }

    public string Sender { get; }
    public string Content { get; set; }
    public DateTime SendDateTime { get; }
    public MessageType MessageType { get; set; }
}