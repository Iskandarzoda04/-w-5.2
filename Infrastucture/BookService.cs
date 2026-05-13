using Domain;
using Dapper;
using Npgsql;
using System.Data;
using Infrastructure.Interfacee;
using Infrastructure.Date;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Infrastructure;

public class BookService : IBookService
{
    private readonly DateContext _context;
    private readonly ILogger<BookService> _logger;



    public BookService(DateContext context, ILogger<BookService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddBook(Book book)
    {
        _logger.LogInformation("Adding a new  book: Title={Title}, Genre={Genre}", book.Title, book.Genre);

        try
        {
             using var con = _context.GetConnection();

        var sql = @"insert into books(title, genre, publicationyear, totalcopies, availablecopies)
                    values(@Title, @Genre, @PublicationYear, @TotalCopies, @AvailableCopies)";

        await con.ExecuteAsync(sql, book);
        

        _logger.LogInformation("Book added successfully: Title={Title}", book.Title);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex,"Error occurred while adding book: Title={Title}", book.Title);
            throw;
        }
        
      
    }

    public async Task DeleteBook(int id)
    {

        _logger.LogInformation("Deleting book with id: id={Id}", id);

        try
        {
        using var con = _context.GetConnection();

        var sql = "delete from books where bookid = @Id";

        await con.ExecuteAsync(sql, new { Id = id });

        _logger.LogInformation("Book deleted successfully: id={Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error  while deleting book: id={Id}", id);
            throw;
        }
    }

    public async Task<List<Book>> GetAllBooks()
    {

        _logger.LogInformation("GetAllBooks started");

        try
        {
        using var con = _context.GetConnection();

        var sql = "select * from books";

         var bk = con.Query<Book>(sql).ToList();
        
     
      _logger.LogError("GetAllBooks {Count} books found", bk.Count);

            return bk;
        }

     catch (Exception ex)
        {
            _logger.LogError(ex, "Error while reryjstrewaetrieving all books");
            throw;
        }
    }

    public async Task<Book?> GetBookById(int id)
    {
        _logger.LogInformation("GetBookById  strated: id={Id}", id);
        try
        {
        using var con = _context.GetConnection();

        var sql = "select * from books where bookid = @Id";

        var bk = await con.QuerySingleOrDefaultAsync<Book>(sql, new { Id = id});
          if (bk == null)
                _logger.LogWarning("Book not found: Id={Id}", id);
            else
                _logger.LogInformation("Book found: Id={Id}, Title={Title}", id, bk.Title);

            return bk;
        return bk;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving book: Id={Id}", id);
            throw;
        }
        }
    

    public async Task UpdateBook(Book book)
    {
        _logger.LogInformation("Updating book: id={Id}, Title={Title}", book.BookId,  book.Title);


        try
        {
            using var con = _context.GetConnection();

        var sql = @"update books
                    set title = @Title,
                     genre = @Genre,
                     publicationyear = @PublicationYear,
                     totalcopies = @TotalCopies,
                    availablecopies = @AvailableCopies
                    where bookid = @BookId";

        await con.ExecuteAsync(sql, book);

        _logger.LogInformation("Book update successfully: id={id}, Title={Title}", book.BookId, book.Title);

        }

        catch(Exception ex)
        {
            _logger.LogError(ex, "Error while updating book: Id={Id}", book.BookId);
            throw;
        }
        
    }

public async Task<dynamic> GetMostPopularBook()
{
    _logger.LogInformation("GetMostPopularBook started");

    try
    {
        using var con = _context.GetConnection();

        var sql = @"select b.title as Title, count(br.borrowingid) as TotalBorrow
                    from books b
                    join borrowings br on b.bookid = br.bookid
                    group by b.title
                    order by TotalBorrow desc
                    limit 1";

        var bk = await con.QuerySingleOrDefaultAsync(sql);

        if (bk == null)
            _logger.LogWarning("Most popular book not found");
        else
            _logger.LogInformation("Most popular book found: Title={Title}", (string)bk.Title);

        return bk;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error while retrieving most popular book");
        throw;
    }
}

   
}