using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore_Lib
{
    public class DbClient : IDbClient
    {
        private readonly IMongoCollection<Book> _book;
        public DbClient(IOptions<BookStoreDbConfig> bookstoreDbConfig)
        {
             var client = new MongoClient(bookstoreDbConfig.Value.Connection_String);
            var database = client.GetDatabase(bookstoreDbConfig.Value.Database_Name);
            _book = database.GetCollection<Book>(bookstoreDbConfig.Value.Books_Collection_Name);
        }

        public IMongoCollection<Book> GetBookCollection() => _book;
    }
}
