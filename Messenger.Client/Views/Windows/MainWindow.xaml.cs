using System;
using Messenger.Client.ViewModels;
using Messenger.Client.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Appearance;

namespace Messenger.Client.Views.Windows;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow(IServiceProvider serviceProvider,
        MainWindowViewModel viewModel)
    {
        SystemThemeWatcher.Watch(this);

        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();

        RootFrame.Navigate(serviceProvider.GetRequiredService<MessengerView>());
    }

    public MainWindowViewModel ViewModel { get; }
}