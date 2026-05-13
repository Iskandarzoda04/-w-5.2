using Domain;
using Infrastructure;
using Infrastructure.Interfacee;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController 
{
    private readonly IBookService _bookService;
    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<List<Book>> Get()
    {
        return await _bookService.GetAllBooks();
    }

    [HttpGet("{id:int}")]
    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _bookService.GetBookById(id);
    }

    [HttpPost]
    public async Task AddAsync(Book book)
    {
        await _bookService.AddBook(book);
    }

    [HttpPut]
    public async Task UpdateAsync(Book book)
    {
        await _bookService.UpdateBook(book);
    }

    [HttpDelete("{id:int}")]
    public async Task DeleteAsync(int id)
    {
        await _bookService.DeleteBook(id);
    }
}

internal class p
{
}