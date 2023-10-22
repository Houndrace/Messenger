using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Messenger.Services;
using Messenger.ViewModels;
using Messenger.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;

namespace Messenger
{
    /// <summary>
    /// Interaction logic for App.xaml
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
            serviceCollection.AddSingleton<IServerService, ServerService>();

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
}