using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Messenger.Client.Models;
using Messenger.Client.Services;

namespace Messenger.Client.ViewModels;

public partial class MessengerViewModel : ObservableObject
{
    private const int ServerPort = 12345;
    private const string ServerIpAddress = "127.0.0.1";
    private const string Sender = "Houndrace";

    private readonly IClientService _clientService;
    [ObservableProperty] 
    [NotifyCanExecuteChangedFor(nameof(SendMessageCommand))]
    private string? _messageContent;
    [ObservableProperty] private Message? _selectedItem;
    [ObservableProperty] private bool _isSendMessageButtonEnabled;

    public MessengerViewModel(IClientService clientService)
    {
        _clientService = clientService;

        try
        {
            _clientService.ConnectToServer(IPAddress.Parse(ServerIpAddress), ServerPort);
        }
        catch (Exception ex)
        {
            Console.WriteLine("_clientService.ConnectToServer: " + ex.Message + "  " + ex.Source);
        }
        
        _clientService.MessageRecieved += message =>
        {
            message.MessageType = MessageType.Recieved;
            MessageCollection.Add(message);
        };
    }

    public ObservableCollection<Message> MessageCollection { get; } = new();


    private bool CanSendMessage() => !string.IsNullOrWhiteSpace(MessageContent);
    
    [RelayCommand(CanExecute = nameof(CanSendMessage))]
    private void SendMessage()
    {
        var message = new Message(Sender, MessageContent, MessageType.Sended);

        try
        {
            _clientService.SendMessage(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("_clientService.SendMessage: " + ex.Message + "  " + ex.Source);
            return;
        }

        MessageCollection.Add(message);
        MessageContent = null;
        SelectedItem = message;
    }
}