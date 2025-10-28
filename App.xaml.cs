using Library.Services;
using Library.ViewModels;
using Library.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Library
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindowViewModel = new MainWindowViewModel(new SqlDatabaseService(), Application.Current.MainWindow);
            var mainWindow = new MainWindow();
            mainWindow.DataContext = mainWindowViewModel;

            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }
    }
}
