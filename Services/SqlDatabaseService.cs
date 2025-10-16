using Library.Data;
using Library.Models;
using Library.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Library.Services
{
    public class SqlDatabaseService : IDatabaseService
    {
        private LibraryDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
            optionsBuilder.UseSqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["LibraryDatabase"]?.ConnectionString);
            return new LibraryDbContext(optionsBuilder.Options);
        }

        public void AddBook(Book book)
        {
            using (var context = CreateDbContext())
            {
                // EF Core automatically assigns an ID if it's an int and not provided
                // if the database is configured for identity.
                context.Books.Add(book);
                context.SaveChanges();
            }
        }

        public void UpdateBook(Book book)
        {
            using (var context = CreateDbContext())
            {
                var existingBook = context.Books.Find(book.Id);
                if (existingBook != null)
                {
                    existingBook.Title = book.Title;
                    existingBook.Author = book.Author;
                    existingBook.ISBN = book.ISBN;
                    existingBook.PublicationYear = book.PublicationYear;
                    existingBook.Genre = book.Genre;
                    existingBook.AvailableCopies = book.AvailableCopies;

                    context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException($"Book with ID {book.Id} not found for update.");
                }
            }
        }

        public Book GetBookById(int id)
        {
            using (var context = CreateDbContext())
            {
                return context.Books.Find(id);
            }
        }

        public ObservableCollection<Book> GetAllBooks()
        {
            using (var context = CreateDbContext())
            {
                return new ObservableCollection<Book>(context.Books.ToList());
            }
        }
    }
}
