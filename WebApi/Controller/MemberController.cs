using Domain;
using Infrastructure;
using Infrastructure.Interfacee;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/members")]
public class MembersController 
{
     public readonly IMemberService _memberService = new MemberService();
  
    [HttpGet]
    public async Task<List<Member>> Get()
    {
        return await _memberService.GetAllMembers();
    }


    [HttpGet("{id:int}")]
    public async Task<Member?> GetById(int id)
    {
        return await _memberService.GetMemberById(id);
    }

  
    [HttpPost]
    public async Task Add(Member member)
    {
        await _memberService.AddMember(member);
    }


    [HttpPut("{id:int}")]
    public async Task Update(int id, Member member)
    {
        member.MemberId = id;
        await _memberService.UpdateMember(member);
    }

  
    [HttpDelete("{id:int}")]
    public async Task Delete(int id)
    {
        await _memberService.DeleteMember(id);
    }

   
    [HttpGet("active")]
    public async Task<dynamic> GetMostActive()
    {
        return await _memberService.GetMostActiveMember();
    }
}

 