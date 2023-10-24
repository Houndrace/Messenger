using System.Windows;
using System.Windows.Controls;
using Messenger.Client.ViewModels;

namespace Messenger.Client.Views.Pages;

public partial class MessengerView : Page
{
    public MessengerView(MessengerViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();

        MessageField.Focus();
    }

    public MessengerViewModel ViewModel { get; }

    private void ChatListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ChatListView.ScrollIntoView(ChatListView.SelectedItem);
    }

    private void Button_OnClickToRefocus(object sender, RoutedEventArgs e)
    {
        MessageField.Focus();
    }
}