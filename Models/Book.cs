using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book : ICloneable, INotifyPropertyChanged
    {
        private int _id;
        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        private string _title;
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        private string _authorNameForDisplay;
        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(150, ErrorMessage = "Author name cannot be longer than 150 characters.")]
        public string AuthorNameForDisplay
        {
            get { return _authorNameForDisplay; }
            set { _authorNameForDisplay = value; OnPropertyChanged(nameof(AuthorNameForDisplay)); }
        }

        private string _isbn;
        [Required(ErrorMessage = "ISBN is required.")]
        [StringLength(20, ErrorMessage = "ISBN cannot be longer than 20 characters.")]
        [RegularExpression(@"^(?:\d{9}[\dX]|\d{13})$", ErrorMessage = "Invalid ISBN format.")]
        public string ISBN
        {
            get { return _isbn; }
            set { _isbn = value; OnPropertyChanged(nameof(ISBN)); }
        }

        private int? _publicationYear;
        [Range(1000, 2100, ErrorMessage = "Publication year must be between 1000 and 2100.")]
        public int? PublicationYear
        {
            get { return _publicationYear; }
            set { _publicationYear = value; OnPropertyChanged(nameof(PublicationYear)); }
        }

        private string _genre;
        [StringLength(50, ErrorMessage = "Genre cannot be longer than 50 characters.")]
        public string Genre
        {
            get { return _genre; }
            set { _genre = value; OnPropertyChanged(nameof(Genre)); }
        }

        private int _availableCopies;
        [Range(0, int.MaxValue, ErrorMessage = "Available copies cannot be negative.")]
        public int AvailableCopies
        {
            get { return _availableCopies; }
            set { _availableCopies = value; OnPropertyChanged(nameof(AvailableCopies)); }
        }

        [ForeignKey("Author")]
        public int? AuthorId { get; set; }

        [ForeignKey("Publisher")]
        public int? PublisherId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Publisher Publisher { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object Clone()
        {
            return new Book
            {
                Id = this.Id,
                Title = this.Title,
                AuthorId = this.AuthorId,
                ISBN = this.ISBN,
                PublicationYear = this.PublicationYear,
                Genre = this.Genre,
                AvailableCopies = this.AvailableCopies,
                PublisherId = this.PublisherId,
                //Publisher = this.Publisher // Deep clone for Publisher is complex
            };
        }
    }
}