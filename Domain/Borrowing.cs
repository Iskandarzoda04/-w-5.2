namespace Domain;

public class Borrowing
{
    public int BorrowingId {get;set;}
    public int BookId {get;set;}
    public int MemberId {get;set;}
    public DateTime BorrowDate {get;set;}
    public DateTime DueDate {get;set;}
    public DateTime? ReturnDate {get;set;}
    public decimal Fine { get; set; }
}