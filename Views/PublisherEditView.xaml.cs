using Library.Services;
using Library.ViewModels;
using System;
using System.ComponentModel; // For DesignerProperties
using System.Windows;

namespace Library.Views
{
    /// <summary>
    /// Interaction logic for PublisherEditView.xaml
    /// </summary>
    public partial class PublisherEditView : Window
    {
        public PublisherEditView()
        {
            InitializeComponent();

            if (DesignerProperties.IsInDesignMode(this))
            {
                var mockService = new MockDatabaseService();
                var designViewModel = new PublisherEditViewModel(null, mockService, null);
                this.DataContext = designViewModel;
            }
            else
            {
                var databaseService = new SqlDatabaseService();
                var ownerWindow = Application.Current.MainWindow;
                var publisherViewModel = new PublisherEditViewModel(null, databaseService, ownerWindow);
                this.DataContext = publisherViewModel;
            }
        }
    }
}