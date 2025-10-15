using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Library.Models;
using Library.Services; // We'll create this for DB operations later

namespace Library.ViewModels
{
    public interface IDatabaseService
    {
        void AddBook(Book book);
        void UpdateBook(Book book);
        Book GetBookById(int id);
    }

    public class BookEditViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private Book _book;
        public Book Book
        {
            get { return _book; }
            set
            {
                _book = value;
                OnPropertyChanged(nameof(Book));
                ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
                ValidateBookProperties();
            }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(SaveBook, CanSaveBook)); }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(CancelEdit)); }
        }

        private readonly IDatabaseService _databaseService;
        private readonly Window _ownerWindow;

        public BookEditViewModel(Book bookToEdit, IDatabaseService databaseService, Window ownerWindow)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _ownerWindow = ownerWindow ?? throw new ArgumentNullException(nameof(ownerWindow));

            Book = bookToEdit?.Clone() ?? new Book();

            ValidateBookProperties();
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
            return !HasErrors && Book != null && !string.IsNullOrWhiteSpace(Book.Title) && !string.IsNullOrWhiteSpace(Book.Author);
        }

        private void CancelEdit(object parameter)
        {
            _ownerWindow?.Close();
        }

        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();
        public bool HasErrors => _errors.Count > 0;

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                _errors.Remove(columnName);

                if (Book == null) return null;

                var propertyInfo = Book.GetType().GetProperty(columnName);
                if (propertyInfo == null) return null;

                var value = propertyInfo.GetValue(Book);
                var validationContext = new ValidationContext(Book) { MemberName = columnName };
                var validationResults = new List<ValidationResult>();

                bool isValid = Validator.TryValidateProperty(value, validationContext, validationResults);

                if (validationResults.Any())
                {
                    string errorMessages = string.Join(Environment.NewLine, validationResults.Select(vr => vr.ErrorMessage));
                    _errors[columnName] = errorMessages;
                    return errorMessages;
                }

                return null;
            }
        }


        private void ValidateBookProperties()
        {

            foreach (var prop in Book.GetType().GetProperties())
            {
                if (prop.GetCustomAttributes(typeof(ValidationAttribute), true).Any())
                {
                    var error = this[prop.Name];
                }
            }
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        private EventHandler _canExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { _canExecuteChanged += value; }
            remove { _canExecuteChanged -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            _canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public static class ValidationExtensions
    {
        public static bool Any(this IEnumerable<ValidationResult> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            return source.Any();
        }
    }

    public static class BookExtensions
    {
        public static Book Clone(this Book book)
        {
            if (book == null) return null;
            return new Book
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear,
                Genre = book.Genre,
                AvailableCopies = book.AvailableCopies
            };
        }
    }
}