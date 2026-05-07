using Domain;
using System.Data;

namespace Infrastructure.Interfacee;

public interface IBorrowingService
{
    Task AddBorrowing(Borrowing borrowing);
    Task UpdateBorrowing(Borrowing borrowing);
    Task DeleteBorrowing(int id);
    Task<Borrowing?> GetBorrowingById(int id);
    Task<List<Borrowing>> GetAllBorrowings();
    Task<int> GetTotalBorrowedBooks();
    Task<decimal> GetAverageFine();
    Task<List<dynamic>> GetNotReturnedBooks();
    Task<int> GetMembersWithBorrowings();
}