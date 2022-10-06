using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Bookstore_Lib
{
    public class BookServices : IBookServices
    {
        private readonly IMongoCollection<Book> _book;
        public BookServices(IDbClient dbClient)
        {
          _book =  dbClient.GetBookCollection();
        }

        public Book AddBook(Book book)
        {
           _book.InsertOne(book);
            return book;    
        }

        public List<Book> GetAllBooks() => _book.Find(book => true).ToList();
        }
}
