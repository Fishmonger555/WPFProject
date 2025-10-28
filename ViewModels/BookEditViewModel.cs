using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Library.Models;
using Library.Services;
using Library.Commands;

namespace Library.ViewModels
{
    public class BookEditViewModel : INotifyPropertyChanged
    {
        private Book _book;
        public Book Book
        {
            get { return _book; }
            set { _book = value; OnPropertyChanged(nameof(Book)); }
        }

        private IDatabaseService _databaseService;
        private Window _ownerWindow;

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
                if (_book != null)
                {
                    if (_selectedAuthor != null)
                    {
                        _book.AuthorId = _selectedAuthor.Id;
                        _book.Author = _selectedAuthor;
                        _book.AuthorNameForDisplay = _selectedAuthor.Name;
                    }
                    else
                    {
                        _book.AuthorId = null;
                        _book.Author = null;
                        _book.AuthorNameForDisplay = string.Empty;
                    }
                }
                ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
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
                if (_book != null)
                {
                    if (_selectedPublisher != null)
                    {
                        _book.PublisherId = _selectedPublisher.Id;
                        _book.Publisher = _selectedPublisher;
                    }
                    else
                    {
                        _book.PublisherId = null;
                        _book.Publisher = null;
                    }
                }
                ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public BookEditViewModel(Book bookToEdit, IDatabaseService databaseService, Window ownerWindow)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService), "Database service cannot be null.");
            _ownerWindow = ownerWindow;

            Book = bookToEdit?.Clone() as Book ?? new Book();

            LoadAuthors();
            LoadPublishers();

            if (Book != null)
            {
                SelectedAuthor = Authors?.FirstOrDefault(a => a.Id == Book.AuthorId);
                SelectedPublisher = Publishers?.FirstOrDefault(p => p.Id == Book.PublisherId);
            }
            else
            {
                Book = new Book();
            }

            SaveCommand = new RelayCommand(SaveBook, CanSaveBook);
            CancelCommand = new RelayCommand(CancelEdit);
        }

        public BookEditViewModel()
            : this(new Book { Title = "Design-Time Book", AuthorNameForDisplay = "Designer Author" }, new MockDatabaseService(), null)
        {
        }

        private void LoadAuthors()
        {
            if (_databaseService != null)
            {
                Authors = _databaseService.GetAllAuthors();
                if (Book?.AuthorId != null && Authors != null)
                {
                    SelectedAuthor = Authors.FirstOrDefault(a => a.Id == Book.AuthorId);
                }
                else
                {
                    SelectedAuthor = null;
                }
            }
            else
            {
                Authors = new ObservableCollection<Author>();
                SelectedAuthor = null;
            }
        }

        private void LoadPublishers()
        {
            if (_databaseService != null)
            {
                Publishers = _databaseService.GetAllPublishers();
                if (Book?.PublisherId != null && Publishers != null)
                {
                    SelectedPublisher = Publishers.FirstOrDefault(p => p.Id == Book.PublisherId);
                }
                else
                {
                    SelectedPublisher = null;
                }
            }
            else
            {
                Publishers = new ObservableCollection<Publisher>();
                SelectedPublisher = null;
            }
        }

        private void SaveBook(object parameter)
        {
            if (Book.Id == 0)
            {
                _databaseService.AddBook(Book);
            }
            else
            {
                _databaseService.UpdateBook(Book);
            }

            _ownerWindow?.Close();
        }

        private bool CanSaveBook(object parameter)
        {
            if (Book == null || string.IsNullOrWhiteSpace(Book.Title) || string.IsNullOrWhiteSpace(Book.AuthorNameForDisplay))
            {
                return false;
            }

            bool authorValid = Book.AuthorId.HasValue;
            bool publisherValid = Book.PublisherId.HasValue || Book.PublisherId == null;

            return authorValid && publisherValid;
        }

        private void CancelEdit(object parameter)
        {
            _ownerWindow?.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}