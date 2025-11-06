using Library.Services;
using Library.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Markup;

namespace Library.Views
{
    /// <summary>
    /// Interaction logic for AuthorEditView.xaml
    /// </summary>
    public partial class AuthorEditView : Window
    {
        public AuthorEditView()
        {
            InitializeComponent();

            if (DesignerProperties.IsInDesignMode(this))
            {
                var mockService = new MockDatabaseService();
                var designViewModel = new AuthorEditViewModel(null, mockService, null);
                this.DataContext = designViewModel;
            }
            else
            {
                var databaseService = new SqlDatabaseService();
                var ownerWindow = Application.Current.MainWindow;

                var authorViewModel = new AuthorEditViewModel(null, databaseService, ownerWindow);
                this.DataContext = authorViewModel;
            }
        }
    }
}