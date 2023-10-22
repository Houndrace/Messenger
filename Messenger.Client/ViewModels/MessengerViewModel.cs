using System;
using System.Collections.ObjectModel;
using System.Net;
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
    [ObservableProperty] private string? _messageContent;

    public MessengerViewModel(IClientService clientService)
    {
        _clientService = clientService;

        try
        {
            _clientService.ConnectToServer(IPAddress.Parse(ServerIpAddress), ServerPort);
        }
        catch (Exception ex)
        {
            
            Console.WriteLine("_clientService.ConnectToServer: " + ex.Message);
        }

        _clientService.MessageRecieved += message => MessageCollection.Add(message);
    }

    public ObservableCollection<Message> MessageCollection { get; } = new();

    [RelayCommand]
    private void OnSendMessage()
    {
        // TODO:сделать верификацию
        var message = new Message(Sender, MessageContent, MessageType.Sended);

        try
        {
            _clientService.SendMessage(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("_clientService.SendMessage: " + ex.Message);
            return;
        }

        MessageCollection.Add(message);
        MessageContent = null;
    }
}