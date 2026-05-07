namespace Domain;

public class Borrowing
{
    public int BorrowingId {get;set;}
    public int BookId {get;set;}
    public int MemberId {get;set;}
    public DateOnly BorrowDate {get;set;}
    public DateOnly DueDate {get;set;}
    public DateOnly? ReturnDate {get;set;}
    public decimal Fine { get; set; }
}