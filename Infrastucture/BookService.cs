using Domain;
using Dapper;
using Npgsql;
using System.Data;
using Infrastructure.Interfacee;
using Infrastructure.Date;

namespace Infrastructure;

public class BookService : IBookService
{
    public readonly DateContext context = new DateContext();

    public async Task AddBook(Book book)
    {
        using var con = context.GetConnection();

        var sql = @"insert into books(title, genre, publicationyear, totalcopies, availablecopies)
                    values(@Title, @Genre, @PublicationYear, @TotalCopies, @AvailableCopies)";

        await con.ExecuteAsync(sql, book);
    }

    public async Task DeleteBook(int id)
    {
        using var con = context.GetConnection();

        var sql = "delete from books where bookid = @Id";

        await con.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<List<Book>> GetAllBooks()
    {
        using var con = context.GetConnection();

        var sql = "select * from books";

         var bk = con.Query<Book>(sql).ToList();
        
         return bk;
    }

    public async Task<Book?> GetBookById(int id)
    {
        using var con = context.GetConnection();

        var sql = "select * from books where bookid = @Id";

        var bk = await con.QuerySingleOrDefaultAsync<Book>(sql, new { Id = id});
        return bk;
    }

    public async Task UpdateBook(Book book)
    {
        using var con = context.GetConnection();

        var sql = @"update books
                    set title = @Title,
                     genre = @Genre,
                     publicationyear = @PublicationYear,
                     totalcopies = @TotalCopies,
                    availablecopies = @AvailableCopies
                    where bookid = @BookId";

        await con.ExecuteAsync(sql, book);
    }

public async Task<dynamic> GetMostPopularBook()
{
    using var con = context.GetConnection();

    var sql = @"select b.title, count(br.borrowingid) as total_borrow
                from books b
                join borrowings br on b.bookid = br.bookid
                group by b.title
                order by total_borrow desc
                limit 1";

    var bk = await con.QuerySingleOrDefaultAsync(sql);

    return bk;
}

    Task IBookService.GetMostPopularBook()
    {
        return GetMostPopularBook();
    }

}