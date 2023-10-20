using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Messenger.ViewModels;
using Messenger.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Appearance;

namespace Messenger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindowViewModel ViewModel { get; }

        public MainWindow(IServiceProvider serviceProvider, MainWindowViewModel viewModel)
        {
            SystemThemeWatcher.Watch(this);

            ViewModel = viewModel;
            DataContext = this;
            
            InitializeComponent();

            RootFrame.Navigate(serviceProvider.GetRequiredService<MessengerView>());
        }
    }
}