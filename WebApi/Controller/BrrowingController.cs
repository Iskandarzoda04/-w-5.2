using Domain;
using Infrastructure;
using Infrastructure.Interfacee;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/borrowings")]
public class BorrowingsController 
{
    private readonly IBorrowingService _service;
    private readonly ILogger<BorrowingsController> _Logger;

    public BorrowingsController(IBorrowingService service, ILogger<BorrowingsController> logger)
    {
        _service = service;
        _Logger = logger;
    }

  
    [HttpGet]
    public async Task<List<Borrowing>> Get()
    {
        return await _service.GetAllBorrowings();
    }

   
    [HttpGet("{id:int}")]
    public async Task<Borrowing?> GetById(int id)
    {
        return await _service.GetBorrowingById(id);
    }

   
    [HttpPost]
    public async Task Add(Borrowing borrowing)
    {
        await _service.AddBorrowing(borrowing);
    }

  
    [HttpPut("{id:int}")]
    public async Task Update(int id, Borrowing borrowing)
    {
        borrowing.BorrowingId = id;
        await _service.UpdateBorrowing(borrowing);
    }

    
    [HttpDelete("{id:int}")]
    public async Task Delete(int id)
    {
        await _service.DeleteBorrowing(id);
    }

   
    [HttpGet("total")]
    public async Task<int> GetTotal()
    {
        return await _service.GetTotalBorrowedBooks();
    }

  
    [HttpGet("average")]
    public async Task<decimal> GetAverageFine()
    {
        return await _service.GetAverageFine();
    }

   
    [HttpGet("returne")]
    public async Task<List<dynamic>> GetNotReturned()
    {
        return await _service.GetNotReturnedBooks();
    }

    [HttpGet("members")]
    public async Task<int> GetMembersCount()
    {
        return await _service.GetMembersWithBorrowings();
    }
}

internal class BrorrowingsController
{
}