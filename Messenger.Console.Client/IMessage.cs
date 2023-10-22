using System;

namespace Messenger.Models;

public interface IMessage
{
    string Sender { get; }
    string Content { get; set; }
    DateTime SendDateTime { get; }
    MessageType MessageType { get; set; }
}

public enum MessageType
{
    Sended,
    Recieved
}