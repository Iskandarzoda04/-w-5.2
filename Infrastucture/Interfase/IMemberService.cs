using Domain;
using System.Data;

namespace Infrastructure.Interfacee;

public interface IMemberService
{
    Task AddMember(Member member);
    Task UpdateMember(Member member);
    Task DeleteMember(int id);
    Task<Member?> GetMemberById(int id);
    Task<List<Member>> GetAllMembers();
     Task<dynamic> GetMostActiveMember();
}