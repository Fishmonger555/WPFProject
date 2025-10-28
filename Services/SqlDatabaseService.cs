using Library.Models;
using Library.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Configuration;

namespace Library.Services
{
    public class SqlDatabaseService : IDatabaseService
    {
        private LibraryDbContext CreateDbContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["LibraryDatabase"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'LibraryDatabase' not found in App.config or is empty.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new LibraryDbContext(optionsBuilder.Options);
        }

        public ObservableCollection<Book> GetAllBooks()
        {
            using (var context = CreateDbContext())
            {
                return new ObservableCollection<Book>(context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Publisher)
                    .ToList());
            }
        }

        public Book GetBookById(int id)
        {
            using (var context = CreateDbContext())
            {
                return context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Publisher)
                    .FirstOrDefault(b => b.Id == id);
            }
        }

        public void AddBook(Book book)
        {
            using (var context = CreateDbContext())
            {
                if (book.AuthorId.HasValue)
                {
                    var author = context.Authors.Find(book.AuthorId.Value);
                    if (author != null)
                    {
                        book.Author = author;
                    }
                    else if (book.Author != null)
                    {
                        context.Attach(book.Author);
                    }
                }

                if (book.PublisherId.HasValue)
                {
                    var publisher = context.Publishers.Find(book.PublisherId.Value);
                    if (publisher != null)
                    {
                        book.Publisher = publisher;
                    }
                    else if (book.Publisher != null)
                    {
                        context.Attach(book.Publisher);
                    }
                }

                context.Books.Add(book);
                context.SaveChanges();
            }
        }

        public void UpdateBook(Book book)
        {
            using (var context = CreateDbContext())
            {
                var existingBook = context.Books
                                        .Include(b => b.Author)
                                        .Include(b => b.Publisher)
                                        .FirstOrDefault(b => b.Id == book.Id);

                if (existingBook != null)
                {
                    existingBook.Title = book.Title;
                    existingBook.AuthorNameForDisplay = book.AuthorNameForDisplay;
                    existingBook.ISBN = book.ISBN;
                    existingBook.PublicationYear = book.PublicationYear;
                    existingBook.Genre = book.Genre;
                    existingBook.AvailableCopies = book.AvailableCopies;

                    existingBook.AuthorId = book.AuthorId;
                    existingBook.PublisherId = book.PublisherId;

                    if (book.Author != null && book.Author.Id != 0)
                    {
                        existingBook.Author = context.Authors.Find(book.Author.Id) ?? book.Author;
                    }
                    else
                    {
                        existingBook.Author = null;
                        existingBook.AuthorId = null;
                    }

                    if (book.Publisher != null && book.Publisher.Id != 0)
                    {
                        existingBook.Publisher = context.Publishers.Find(book.Publisher.Id) ?? book.Publisher;
                    }
                    else
                    {
                        existingBook.Publisher = null;
                        existingBook.PublisherId = null;
                    }

                    context.SaveChanges();
                }
            }
        }

        public ObservableCollection<Author> GetAllAuthors()
        {
            using (var context = CreateDbContext())
            {
                return new ObservableCollection<Author>(context.Authors.ToList());
            }
        }

        public Author GetAuthorById(int id)
        {
            using (var context = CreateDbContext())
            {
                return context.Authors.Find(id);
            }
        }

        public void AddAuthor(Author author)
        {
            using (var context = CreateDbContext())
            {
                context.Authors.Add(author);
                context.SaveChanges();
            }
        }

        public void UpdateAuthor(Author author)
        {
            using (var context = CreateDbContext())
            {
                var existingAuthor = context.Authors.Find(author.Id);
                if (existingAuthor != null)
                {
                    existingAuthor.Name = author.Name;
                    existingAuthor.Biography = author.Biography;
                    existingAuthor.Nationality = author.Nationality;
                    context.SaveChanges();
                }
            }
        }

        public ObservableCollection<Publisher> GetAllPublishers()
        {
            using (var context = CreateDbContext())
            {
                return new ObservableCollection<Publisher>(context.Publishers.ToList());
            }
        }

        public Publisher GetPublisherById(int id)
        {
            using (var context = CreateDbContext())
            {
                return context.Publishers.Find(id);
            }
        }

        public void AddPublisher(Publisher publisher)
        {
            using (var context = CreateDbContext())
            {
                context.Publishers.Add(publisher);
                context.SaveChanges();
            }
        }

        public void UpdatePublisher(Publisher publisher)
        {
            using (var context = CreateDbContext())
            {
                var existingPublisher = context.Publishers.Find(publisher.Id);
                if (existingPublisher != null)
                {
                    existingPublisher.Name = publisher.Name;
                    existingPublisher.Address = publisher.Address;
                    existingPublisher.ContactEmail = publisher.ContactEmail;
                    context.SaveChanges();
                }
            }
        }
    }
}