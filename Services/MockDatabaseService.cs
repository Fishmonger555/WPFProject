using Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Library.Services
{
    public class MockDatabaseService : IDatabaseService
    {
        private List<Book> _books = new List<Book>();
        private int _nextBookId = 1;

        private List<Author> _mockAuthors = new List<Author>();
        private int _nextAuthorId = 1;

        private List<Publisher> _mockPublishers = new List<Publisher>();
        private int _nextPublisherId = 1;

        public MockDatabaseService()
        {
            var author1 = new Author { Id = 1, Name = "George Orwell", Nationality = "British" };
            var author2 = new Author { Id = 2, Name = "Jane Austen", Nationality = "British" };
            _mockAuthors.Add(author1);
            _mockAuthors.Add(author2);
            _nextAuthorId = 3;

            var publisher1 = new Publisher { Id = 1, Name = "Penguin Books", ContactEmail = "info@penguin.com" };
            var publisher2 = new Publisher { Id = 2, Name = "HarperCollins", ContactEmail = "info@harpercollins.com" };
            _mockPublishers.Add(publisher1);
            _mockPublishers.Add(publisher2);
            _nextPublisherId = 3;

            var book1 = new Book
            {
                Id = 1,
                Title = "1984",
                AuthorNameForDisplay = author1.Name,
                AuthorId = author1.Id,
                Author = author1,
                PublisherId = publisher1.Id,
                Publisher = publisher1,
                ISBN = "978-0451524935",
                PublicationYear = 1949,
                Genre = "Dystopian",
                AvailableCopies = 5
            };
            _books.Add(book1);

            var book2 = new Book
            {
                Id = 2,
                Title = "Pride and Prejudice",
                AuthorNameForDisplay = author2.Name,
                AuthorId = author2.Id,
                Author = author2,
                PublisherId = publisher2.Id,
                Publisher = publisher2,
                ISBN = "978-0141439518",
                PublicationYear = 1813,
                Genre = "Romance",
                AvailableCopies = 3
            };
            _books.Add(book2);
            _nextBookId = 3;
        }

        public ObservableCollection<Book> GetAllBooks()
        {
            return new ObservableCollection<Book>(_books.Select(b => b.Clone() as Book).Where(b => b != null));
        }

        public Book GetBookById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id)?.Clone() as Book;
        }

        public void AddBook(Book book)
        {
            var bookToAdd = book.Clone() as Book;
            if (bookToAdd != null)
            {
                bookToAdd.Id = _nextBookId++;
                _books.Add(bookToAdd);
                Console.WriteLine($"Mock Added Book: {bookToAdd.Title} (ID: {bookToAdd.Id})");
            }
        }

        public void UpdateBook(Book book)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook != null)
            {
                var bookToUpdate = book.Clone() as Book;
                if (bookToUpdate != null)
                {
                    existingBook.Title = bookToUpdate.Title;
                    existingBook.AuthorNameForDisplay = bookToUpdate.AuthorNameForDisplay;
                    existingBook.AuthorId = bookToUpdate.AuthorId;
                    existingBook.Author = bookToUpdate.Author;
                    existingBook.ISBN = bookToUpdate.ISBN;
                    existingBook.PublicationYear = bookToUpdate.PublicationYear;
                    existingBook.Genre = bookToUpdate.Genre;
                    existingBook.AvailableCopies = bookToUpdate.AvailableCopies;
                    existingBook.PublisherId = bookToUpdate.PublisherId;
                    existingBook.Publisher = bookToUpdate.Publisher;
                    Console.WriteLine($"Mock Updated Book: {existingBook.Title} (ID: {existingBook.Id})");
                }
            }
            else
            {
                AddBook(book);
            }
        }

        public ObservableCollection<Author> GetAllAuthors()
        {
            return new ObservableCollection<Author>(_mockAuthors.Select(a => a.Clone() as Author).Where(a => a != null));
        }

        public Author GetAuthorById(int id)
        {
            return _mockAuthors.FirstOrDefault(a => a.Id == id)?.Clone() as Author;
        }

        public void AddAuthor(Author author)
        {
            var authorToAdd = author.Clone() as Author;
            if (authorToAdd != null)
            {
                authorToAdd.Id = _nextAuthorId++;
                _mockAuthors.Add(authorToAdd);
                Console.WriteLine($"Mock Added Author: {authorToAdd.Name} (ID: {authorToAdd.Id})");
            }
        }

        public void UpdateAuthor(Author author)
        {
            var existingAuthor = _mockAuthors.FirstOrDefault(a => a.Id == author.Id);
            if (existingAuthor != null)
            {
                var authorToUpdate = author.Clone() as Author;
                if (authorToUpdate != null)
                {
                    existingAuthor.Name = authorToUpdate.Name;
                    existingAuthor.Biography = authorToUpdate.Biography;
                    existingAuthor.Nationality = authorToUpdate.Nationality;
                    Console.WriteLine($"Mock Updated Author: {existingAuthor.Name} (ID: {existingAuthor.Id})");
                }
            }
            else
            {
                AddAuthor(author);
            }
        }

        public ObservableCollection<Publisher> GetAllPublishers()
        {
            return new ObservableCollection<Publisher>(_mockPublishers.Select(p => p.Clone() as Publisher).Where(p => p != null));
        }

        public Publisher GetPublisherById(int id)
        {
            return _mockPublishers.FirstOrDefault(p => p.Id == id)?.Clone() as Publisher;
        }

        public void AddPublisher(Publisher publisher)
        {
            var publisherToAdd = publisher.Clone() as Publisher;
            if (publisherToAdd != null)
            {
                publisherToAdd.Id = _nextPublisherId++;
                _mockPublishers.Add(publisherToAdd);
                Console.WriteLine($"Mock Added Publisher: {publisherToAdd.Name} (ID: {publisherToAdd.Id})");
            }
        }

        public void UpdatePublisher(Publisher publisher)
        {
            var existingPublisher = _mockPublishers.FirstOrDefault(p => p.Id == publisher.Id);
            if (existingPublisher != null)
            {
                var publisherToUpdate = publisher.Clone() as Publisher;
                if (publisherToUpdate != null)
                {
                    existingPublisher.Name = publisherToUpdate.Name;
                    existingPublisher.Address = publisherToUpdate.Address;
                    existingPublisher.ContactEmail = publisherToUpdate.ContactEmail;
                    Console.WriteLine($"Mock Updated Publisher: {existingPublisher.Name} (ID: {existingPublisher.Id})");
                }
            }
            else
            {
                AddPublisher(publisher);
            }
        }
    }
}