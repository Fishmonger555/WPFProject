using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Library.Models;
using Library.Services;
using Library.Commands;

namespace Library.ViewModels
{
    public class AuthorEditViewModel : INotifyPropertyChanged
    {
        private Author _author;
        public Author Author
        {
            get { return _author; }
            set { _author = value; OnPropertyChanged(nameof(Author)); }
        }

        private IDatabaseService _databaseService;
        private Window _ownerWindow;

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public AuthorEditViewModel(Author authorToEdit, IDatabaseService databaseService, Window ownerWindow)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService), "Database service cannot be null.");
            _ownerWindow = ownerWindow;

            Author = authorToEdit?.Clone() as Author ?? new Author();

            SaveCommand = new RelayCommand(SaveAuthor, CanSaveAuthor);
            CancelCommand = new RelayCommand(CancelEdit);
        }

        public AuthorEditViewModel()
            : this(new Author { Name = "Design-Time Author" }, new MockDatabaseService(), null)
        {
        }

        private void SaveAuthor(object parameter)
        {
            if (Author.Id == 0)
            {
                _databaseService.AddAuthor(Author);
            }
            else
            {
                _databaseService.UpdateAuthor(Author);
            }
            _ownerWindow?.Close();
        }

        private bool CanSaveAuthor(object parameter)
        {
            return Author != null && !string.IsNullOrWhiteSpace(Author.Name);
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