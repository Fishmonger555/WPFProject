using Library.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Library.Views
{
    /// <summary>
    /// Interaction logic for BookEditView.xaml
    /// </summary>
    public partial class BookEditView : Window
    {
        public BookEditViewModel ViewModel { get; private set; }

        public BookEditView(BookEditViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            this.DataContext = ViewModel;
        }
    }
}