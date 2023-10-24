using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Messenger.Models;
using Messenger.Services;

namespace Messenger.ViewModels;

public partial class MessengerViewModel : ObservableObject
{
    private readonly string _sender = "Ya";

    public MessengerViewModel(IServerService serverService)
    {
        try
        {
            serverService.StartServer();
        }
        catch (Exception e)
        {
             
        }
        
        
        serverService.MessageRecieved += message =>
        {
            MessageCollection.Add(message);
        };
    }

    public ObservableCollection<Message> MessageCollection { get; } = new();

    [RelayCommand]
    private void SendMessage()
    {
        
    }
}