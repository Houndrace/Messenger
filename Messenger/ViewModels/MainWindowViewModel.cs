using CommunityToolkit.Mvvm.ComponentModel;

namespace Messenger.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private string _appTitle = "Сокет мессенджер";
}