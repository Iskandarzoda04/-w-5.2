using Domain;
using Dapper;
using Infrastructure.Interfacee;
using Infrastructure.Date;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class BorrowingService : IBorrowingService
{
    private readonly DateContext _context;
    private readonly ILogger<BorrowingService> _logger;

    public BorrowingService(DateContext context, ILogger<BorrowingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddBorrowing(Borrowing borrowing)
    {
        _logger.LogInformation("Adding a new borrowing: BookId={BookId}, MemberId={MemberId}", 
            borrowing.BookId, borrowing.MemberId);
        try
        {
            using var con = _context.GetConnection();
            var sql = @"insert into borrowings(bookid, memberid, borrowdate, duedate, returndate, fine)
                        values(@BookId, @MemberId, @BorrowDate, @DueDate, @ReturnDate, @Fine)";

            await con.ExecuteAsync(sql, borrowing);

            _logger.LogInformation("Borrowing added successfully: BookId={BookId}, MemberId={MemberId}", 
                borrowing.BookId, borrowing.MemberId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding borrowing: BookId={BookId}, MemberId={MemberId}", 

                borrowing.BookId, borrowing.MemberId);
            throw;
        }
    }

    public async Task DeleteBorrowing(int id)
    {
        _logger.LogInformation("Deleting borrowing: Id={Id}", id);
        try
        {
            using var con = _context.GetConnection();

            var sql = "delete from borrowings where borrowingid = @Id";

            await con.ExecuteAsync(sql, new { Id = id });

            _logger.LogInformation("Borrowing deleted successfully: Id={Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting borrowing: Id={Id}", id);
            throw;
        }
    }

    public async Task<List<Borrowing>> GetAllBorrowings()
    {
        _logger.LogInformation("GetAllBorrowings started");
        try
        {
            using var con = _context.GetConnection();

            var sql = "select * from borrowings";

            var br = con.Query<Borrowing>(sql).ToList();

            _logger.LogInformation("GetAllBorrowings: {Count} borrowings found", br.Count);
            return br;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving all borrowings");
            throw;
        }
    }

    public async Task<Borrowing?> GetBorrowingById(int id)
    {
        _logger.LogInformation("GetBorrowingById started: Id={Id}", id);
        try
        {
            using var con = _context.GetConnection();

            var sql = "select * from borrowings where borrowingid = @Id"
            ;
            var br = await con.QuerySingleOrDefaultAsync<Borrowing>(sql, new { Id = id });

            if (br == null)
                _logger.LogWarning("Borrowing not found: Id={Id}", id);
            else
                _logger.LogInformation("Borrowing found: Id={Id}", id);
            return br;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving borrowing: Id={Id}", id);
            throw;
        }
    }

    public async Task UpdateBorrowing(Borrowing borrowing)
    {
        _logger.LogInformation("Updating borrowing: Id={Id}", borrowing.BorrowingId);
        try
        {
            using var con = _context.GetConnection();
            var sql = @"update borrowings
                        set bookid = @BookId,
                            memberid = @MemberId,
                            borrowdate = @BorrowDate,
                            duedate = @DueDate,
                            returndate = @ReturnDate,
                            fine = @Fine
                        where borrowingid = @BorrowingId";
            await con.ExecuteAsync(sql, borrowing);

            _logger.LogInformation("Borrowing updated successfully: Id={Id}", borrowing.BorrowingId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating borrowing: Id={Id}", borrowing.BorrowingId);
            throw;
        }
    }

    public async Task<int> GetTotalBorrowedBooks()
    {
        _logger.LogInformation("GetTotalBorrowedBooks started");
        try
        {
            using var con = _context.GetConnection();

            var sql = "select count(*) from borrowings";

            var br = await con.ExecuteScalarAsync<int>(sql);

            _logger.LogInformation("GetTotalBorrowedBooks: {Count} total borrowed books", br);
            return br;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while total borrowed books");
            throw;
        }
    }

    public async Task<decimal> GetAverageFine()
    {
        _logger.LogInformation("GetAverageFine started");
        try
        {
            using var con = _context.GetConnection();
            var sql = "select avg(fine) from borrowings";
            var br = await con.ExecuteScalarAsync<decimal>(sql);
            _logger.LogInformation("GetAverageFine: average fine = {Fine}", br);
            return br;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving average fine");
            throw;
        }
    }

    public async Task<List<dynamic>> GetNotReturnedBooks()
    {
        _logger.LogInformation("GetNotReturnedBooks started");
        try
        {
            using var con = _context.GetConnection();
            var sql = @"select b.title, m.fullname
                        from borrowings br
                        join books b on br.bookid = b.bookid
                        join members m on br.memberid = m.memberid
                        where br.returndate is null";
            var br = (await con.QueryAsync(sql)).ToList();
            _logger.LogInformation("GetNotReturnedBooks: {Count} not returned books found", br.Count);
            return br;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving not returned books");
            throw;
        }
    }

    public async Task<int> GetMembersWithBorrowings()
    {
        _logger.LogInformation("GetMembersWithBorrowings started");
        try
        {
            using var con = _context.GetConnection();
            var sql = "select count(memberid) from borrowings";
            var br = await con.ExecuteScalarAsync<int>(sql);
            _logger.LogInformation("GetMembersWithBorrowings: {Count} members with borrowings", br);
            return br;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving members with borrowings");
            throw;
        }
    }
}