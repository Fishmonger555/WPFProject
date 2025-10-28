using Library.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Library.Services
{
    public interface IDatabaseService
    {
        ObservableCollection<Book> GetAllBooks();
        Book GetBookById(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);

        ObservableCollection<Author> GetAllAuthors();
        Author GetAuthorById(int id);
        void AddAuthor(Author author);
        void UpdateAuthor(Author author);

        ObservableCollection<Publisher> GetAllPublishers();
        Publisher GetPublisherById(int id);
        void AddPublisher(Publisher publisher);
        void UpdatePublisher(Publisher publisher);
    }
}