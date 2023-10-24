using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Messenger.Client.Models;
using Messenger.Client.Services;
using Wpf.Ui.Controls;

namespace Messenger.Client.ViewModels;

public partial class MessengerViewModel : ObservableObject
{
    private const int ServerPort = 12345;
    private const string ServerIpAddress = "127.0.0.1";
    private const string Sender = "Houndrace";

    private readonly IClientService _clientService;
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(RefreshConnectionCommand))] private string? _connectionStatus;

    [ObservableProperty] private bool _isSendMessageButtonEnabled;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SendMessageCommand))]
    private string? _messageContent;

    [ObservableProperty] private Message? _selectedItem;


    public MessengerViewModel(IClientService clientService)
    {
        _clientService = clientService;
        
        _clientService.Disconnected += () => { ConnectionStatus = "Не подключено"; };
        _clientService.Connected += () => { ConnectionStatus = "Подключено"; };
        
        try
        {
            _clientService.ConnectToServer(IPAddress.Parse(ServerIpAddress), ServerPort);
        }
        catch (Exception ex)
        {
            Console.WriteLine("_clientService.ConnectToServer: " + ex.Message + "  " + ex.Source + "  " + ex.GetType());
        }

        _clientService.MessageRecieved += message =>
        {
            message.MessageType = MessageType.Recieved;
            MessageCollection.Add(message);
            SelectedItem = message;
        };

        
    }

    public ObservableCollection<Message> MessageCollection { get; } = new();


    private bool CanRefreshConnection()
    {
        return ConnectionStatus == "Подключено" ? false : true;
    }

    [RelayCommand(CanExecute = nameof(CanRefreshConnection))]
    private async Task RefreshConnection()
    {
        try
        {
            _clientService.ConnectToServer(IPAddress.Parse(ServerIpAddress), ServerPort);
        }
        catch (Exception ex)
        {
            var uiMessageBox = new MessageBox
            {
                Title = "Ошибка",
                Content = $"Не удалось подключиться к серверу. {ex.Message}",
                CloseButtonText = "Закрыть"
            };

            await uiMessageBox.ShowDialogAsync();
        }
    }
    
    private bool CanSendMessage()
    {
        return !string.IsNullOrWhiteSpace(MessageContent);
    }
    
    [RelayCommand(CanExecute = nameof(CanSendMessage))]
    private async Task SendMessage()
    {
        var message = new Message(Sender, MessageContent, MessageType.Sended);

        try
        {
            _clientService.SendMessage(message);
        }
        catch (Exception ex)
        {
            var uiMessageBox = new MessageBox
            {
                Title = "Ошибка",
                Content = $"Не удалось подключиться к серверу, чтобы отправить сообщение. {ex.Message}",
                CloseButtonText = "Закрыть"
            };

            await uiMessageBox.ShowDialogAsync();
            return;
        }

        MessageCollection.Add(message);
        MessageContent = null;
        SelectedItem = message;
    }
}