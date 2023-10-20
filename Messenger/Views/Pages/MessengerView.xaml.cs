using System.Windows.Controls;
using Messenger.ViewModels;
using Wpf.Ui.Appearance;

namespace Messenger.Views.Pages;

public partial class MessengerView : Page
{
    public MessengerViewModel ViewModel { get; }

    public MessengerView(MessengerViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        
        InitializeComponent();
    }
}