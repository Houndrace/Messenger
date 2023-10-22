using CommunityToolkit.Mvvm.ComponentModel;

namespace Messenger.Client.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private string _appTitle = "Сокет мессенджер";
}