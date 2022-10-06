using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore_Lib
{
    public interface IBookServices
    {
        List<Book> GetAllBooks();
        Book AddBook(Book book);
    }
}
