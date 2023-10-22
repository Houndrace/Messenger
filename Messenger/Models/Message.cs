using System;

namespace Messenger.Models;

[Serializable]
public class Message : IMessage
{
    public Message()
    {
    }

    public Message(string sender, string content, MessageType messageType)
    {
        Sender = sender;
        Content = content;
        SendDateTime = DateTime.Now;
        MessageType = messageType;
    }

    public string Sender { get; set; }
    public string Content { get; set; }
    public DateTime SendDateTime { get; set; }
    public MessageType MessageType { get; set; }
}