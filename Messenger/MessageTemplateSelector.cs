using System.Windows;
using System.Windows.Controls;
using Messenger.Models;

namespace Messenger;

public class MessageTemplateSelector : DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        var element = container as FrameworkElement;

        if (element is not null && item is not null && item is Message)
        {
            var messageItem = item as Message;


            if (messageItem.MessageType == MessageType.Recieved)
                return element.FindResource("DefaultMessageTemplate") as DataTemplate;
            if (messageItem.MessageType == MessageType.Sended)
                return element.FindResource("SenderMessageTemplate") as DataTemplate;
        }

        return null;
    }
}