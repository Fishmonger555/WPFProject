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
    public class PublisherEditViewModel : INotifyPropertyChanged
    {
        private Publisher _publisher;
        public Publisher Publisher
        {
            get { return _publisher; }
            set { _publisher = value; OnPropertyChanged(nameof(Publisher)); }
        }

        private IDatabaseService _databaseService;
        private Window _ownerWindow;

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public PublisherEditViewModel(Publisher publisherToEdit, IDatabaseService databaseService, Window ownerWindow)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService), "Database service cannot be null.");
            _ownerWindow = ownerWindow;

            Publisher = publisherToEdit?.Clone() as Publisher ?? new Publisher();

            SaveCommand = new RelayCommand(SavePublisher, CanSavePublisher);
            CancelCommand = new RelayCommand(CancelEdit);
        }

        public PublisherEditViewModel()
            : this(new Publisher { Name = "Design-Time Publisher" }, new MockDatabaseService(), null)
        {
        }

        private void SavePublisher(object parameter)
        {
            if (Publisher.Id == 0)
            {
                _databaseService.AddPublisher(Publisher);
            }
            else
            {
                _databaseService.UpdatePublisher(Publisher);
            }
            _ownerWindow?.Close();
        }

        private bool CanSavePublisher(object parameter)
        {
            return Publisher != null && !string.IsNullOrWhiteSpace(Publisher.Name);
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