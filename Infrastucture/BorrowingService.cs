using Domain;
using Dapper;
using Npgsql;
using System.Data;
using Infrastructure.Interfacee;
using Infrastructure.Date;

namespace Infrastructure;

public class BorrowingService : IBorrowingService
{
   public readonly DateContext context = new DateContext();

    public async Task AddBorrowing(Borrowing borrowing)
    {
        using var con = context.GetConnection();

        var sql = @"insert into borrowings(bookid, memberid, borrowdate, duedate, returndate, fine)
                    values(@BookId, @MemberId, @BorrowDate, @DueDate, @ReturnDate, @Fine)";

        await con.ExecuteAsync(sql, borrowing);
    }

    public async Task DeleteBorrowing(int id)
    {
        using var con = context.GetConnection();

        var sql = "delete from borrowings where borrowingid = @Id";

        await con.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<List<Borrowing>> GetAllBorrowings()
    {
        using var con = context.GetConnection();

        var sql = "select * from borrowings";

         var br = con.Query<Borrowing>(sql).ToList();
        
         return br;
    }

    public async Task<Borrowing?> GetBorrowingById(int id)
    {
        using var con = context.GetConnection();

        var sql = "select * from borrowings where borrowingid = @Id";

        var br = await con.QuerySingleOrDefaultAsync<Borrowing>(sql, new { Id = id });
        return br;
    }

    public async Task UpdateBorrowing(Borrowing borrowing)
    {
        using var con = context.GetConnection();

        var sql = @"update borrowings
                    set bookid = @BookId,
                        memberid = @MemberId,
                        borrowdate = @BorrowDate,
                        duedate = @DueDate,
                        returndate = @ReturnDate,
                        fine = @Fine
                    where borrowingid = @BorrowingId";

        await con.ExecuteAsync(sql, borrowing);
    }

  public async Task<int> GetTotalBorrowedBooks()

   {
    using var con = context.GetConnection();

    var sql = "select count(*) from borrowings";

    var br = await con.ExecuteScalarAsync<int>(sql);

    return br;

    }

    public async Task<decimal> GetAverageFine()

    {
    using var con = context.GetConnection();

    var sql = "select avg(fine) from borrowings";

    var br = await con.ExecuteScalarAsync<decimal>(sql);

    return br;
   }

   public async Task<List<dynamic>> GetNotReturnedBooks()
{
    using var con = context.GetConnection();

    var sql = @"select b.title, m.fullname
                from borrowings br
                join books b on br.bookid = b.bookid
                join members m on br.memberid = m.memberid
                where br.returndate is null";

    var br =(await con.QueryAsync(sql)).ToList();
 
    return br;
}

public async Task<int> GetMembersWithBorrowings()
{
    using var con = context.GetConnection();

    var sql = @"select count(memberid)
                from borrowings";

    return await con.ExecuteScalarAsync<int>(sql);
}
  
}