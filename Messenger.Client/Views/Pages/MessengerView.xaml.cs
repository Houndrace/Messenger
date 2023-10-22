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
    }

    public MessengerViewModel ViewModel { get; }
}