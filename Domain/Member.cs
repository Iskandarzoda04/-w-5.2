namespace Domain;

public class Member
{
    public int MemberId {get;set;}
    public string FullName {get;set;} ="";
    public string Phone {get;set;} = "";
    public string Email {get;set;} = "";
    public DateOnly MembershipDate {get;set;}
}