using Domain;
using System.Data;

namespace Infrastructure.Interfacee;

public interface IBookService
{
    Task AddBook(Book book);
    Task UpdateBook(Book book);
    Task DeleteBook(int id);
    Task<Book?> GetBookById(int id);
    Task<List<Book>> GetAllBooks();
    Task<dynamic> GetMostPopularBook();

}