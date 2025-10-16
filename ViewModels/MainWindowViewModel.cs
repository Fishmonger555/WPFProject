using Library.Models;
using Library.Services;
using Library.ViewModels;
using Library.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Library.ViewModels
{
    public class MockDatabaseService : IDatabaseService
    {
        private List<Book> _books = new List<Book>();
        private int _nextId = 1;

        public void AddBook(Book book)
        {
            book.Id = _nextId++;
            _books.Add(book.Clone());
            Console.WriteLine($"Added Book: {book.Title} (ID: {book.Id})");
        }

        public void UpdateBook(Book book)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.ISBN = book.ISBN;
                existingBook.PublicationYear = book.PublicationYear;
                existingBook.Genre = book.Genre;
                existingBook.AvailableCopies = book.AvailableCopies;
                Console.WriteLine($"Updated Book: {existingBook.Title} (ID: {existingBook.Id})");
            }
            else
            {
                AddBook(book);
            }
        }

        public Book GetBookById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id)?.Clone();
        }

        public ObservableCollection<Book> GetAllBooks()
        {
            return new ObservableCollection<Book>(_books.Select(b => b.Clone()));
        }
    }


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
                ((RelayCommand)DeleteBookCommand).RaiseCanExecuteChanged(); // If you add delete later
            }
        }

        public ICommand AddBookCommand { get; private set; }
        public ICommand EditBookCommand { get; private set; }
        public ICommand DeleteBookCommand { get; private set; } // Later
        public ICommand ExitCommand { get; private set; }


        private readonly IDatabaseService _databaseService;

        public MainWindowViewModel()
        {
            _databaseService = new SqlDatabaseService();

            LoadBooks();

            AddBookCommand = new RelayCommand(AddBook);
            EditBookCommand = new RelayCommand(EditBook, CanEditOrDeleteBook);
            // DeleteBookCommand = new RelayCommand(DeleteBook, CanEditOrDeleteBook);
            ExitCommand = new RelayCommand(ExitApplication);
        }

        private void AddBook(object parameter)
        {
            var editViewModel = new BookEditViewModel(null, _databaseService, GetOwnerWindow());
            var editWindow = new BookEditView(editViewModel);
            editWindow.ShowDialog();

            LoadBooks();
        }

        private void EditBook(object parameter)
        {
            if (SelectedBook != null)
            {
                var editViewModel = new BookEditViewModel(SelectedBook, _databaseService, GetOwnerWindow());
                var editWindow = new BookEditView(editViewModel);
                editWindow.ShowDialog();

                LoadBooks();
            }
        }

        private bool CanEditOrDeleteBook(object parameter)
        {
            return SelectedBook != null;
        }

        public void ExitApplication(object parameter)
        {
            Application.Current.Shutdown();
        }

        private void LoadBooks()
        {
            Books = _databaseService.GetAllBooks();
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