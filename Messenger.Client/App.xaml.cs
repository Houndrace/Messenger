using System.Windows;
using Messenger.Client.Services;
using Messenger.Client.ViewModels;
using Messenger.Client.Views.Pages;
using Messenger.Client.Views.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Client;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        var serviceCollection = ConfigureServices();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    private ServiceCollection ConfigureServices()
    {
        ServiceCollection serviceCollection = new();

        // Main window container with navigation
        serviceCollection.AddSingleton<MainWindow>();
        serviceCollection.AddSingleton<MainWindowViewModel>();
        serviceCollection.AddSingleton<IClientService, ClientService>();

        // All other pages and view models
        serviceCollection.AddTransient<MessengerView>();
        serviceCollection.AddTransient<MessengerViewModel>();

        return serviceCollection;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}