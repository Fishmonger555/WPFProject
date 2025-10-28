using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Library.Models;
using Library.Services;
using Library.Views;
using Library.Commands;

namespace Library.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _statusMessage = "Ready";
        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                OnPropertyChanged(nameof(Books));
            }
        }

        private Book _selectedBook;
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
                ((RelayCommand)EditBookCommand).RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors
        {
            get { return _authors; }
            set { _authors = value; OnPropertyChanged(nameof(Authors)); }
        }

        private Author _selectedAuthor;
        public Author SelectedAuthor
        {
            get { return _selectedAuthor; }
            set
            {
                _selectedAuthor = value;
                OnPropertyChanged(nameof(SelectedAuthor));
            }
        }

        private ObservableCollection<Publisher> _publishers;
        public ObservableCollection<Publisher> Publishers
        {
            get { return _publishers; }
            set { _publishers = value; OnPropertyChanged(nameof(Publishers)); }
        }

        private Publisher _selectedPublisher;
        public Publisher SelectedPublisher
        {
            get { return _selectedPublisher; }
            set
            {
                _selectedPublisher = value;
                OnPropertyChanged(nameof(SelectedPublisher));
            }
        }

        public ICommand AddBookCommand { get; private set; }
        public ICommand EditBookCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }

        public ICommand NavigateToAddAuthorCommand { get; private set; }
        public ICommand NavigateToAddPublisherCommand { get; private set; }

        private readonly IDatabaseService _databaseService;

        public MainWindowViewModel()
            : this(new MockDatabaseService(), null)
        {
        }

        public MainWindowViewModel(IDatabaseService databaseService, Window ownerWindow)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService), "Database service cannot be null.");

            LoadBooks();
            LoadAuthors();
            LoadPublishers();

            AddBookCommand = new RelayCommand(AddBook);
            EditBookCommand = new RelayCommand(EditBook, CanEditOrDeleteBook);
            ExitCommand = new RelayCommand(ExitApplication);

            NavigateToAddAuthorCommand = new RelayCommand(NavigateToAddAuthor);
            NavigateToAddPublisherCommand = new RelayCommand(NavigateToAddPublisher);
        }

        private void AddBook(object parameter)
        {
            var editViewModel = new BookEditViewModel(null, _databaseService, GetOwnerWindow());
            var editWindow = new BookEditView();
            editWindow.DataContext = editViewModel;
            editWindow.Owner = GetOwnerWindow();
            editWindow.ShowDialog();

            LoadBooks();
        }

        private void EditBook(object parameter)
        {
            if (SelectedBook != null)
            {
                var editViewModel = new BookEditViewModel(SelectedBook, _databaseService, GetOwnerWindow());
                var editWindow = new BookEditView();
                editWindow.DataContext = editViewModel;
                editWindow.Owner = GetOwnerWindow();
                editWindow.ShowDialog();

                LoadBooks();
            }
        }

        private bool CanEditOrDeleteBook(object parameter)
        {
            return SelectedBook != null;
        }

        private void NavigateToAddAuthor(object parameter)
        {

            MessageBox.Show("Navigate to Add Author functionality not yet implemented.");
        }

        private void NavigateToAddPublisher(object parameter)
        {

            MessageBox.Show("Navigate to Add Publisher functionality not yet implemented.");
        }

        public void ExitApplication(object parameter)
        {
            Application.Current.Shutdown();
        }

        private void LoadBooks()
        {
            if (_databaseService != null)
            {
                Books = _databaseService.GetAllBooks();
                SelectedBook = null;
            }
            else
            {
                Books = new ObservableCollection<Book>();
                SelectedBook = null;
                StatusMessage = "Error: Database service not initialized.";
            }
        }

        private void LoadAuthors()
        {
            if (_databaseService != null)
            {
                Authors = _databaseService.GetAllAuthors();
            }
            else
            {
                Authors = new ObservableCollection<Author>();
            }
        }

        private void LoadPublishers()
        {
            if (_databaseService != null)
            {
                Publishers = _databaseService.GetAllPublishers();
            }
            else
            {
                Publishers = new ObservableCollection<Publisher>();
            }
        }

        private Window GetOwnerWindow()
        {
            return Application.Current.MainWindow;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}