using Domain;
using Dapper;
using Npgsql;
using System.Data;
using Infrastructure.Interfacee;
using Infrastructure.Date;

namespace Infrastructure;

public class MemberService : IMemberService
{
   public readonly DateContext context = new DateContext();

    public async Task AddMember(Member member)
    {
        using var con = context.GetConnection();

        var sql = @"insert into members(fullname, phone, email, membershipdate)
                    values(@FullName, @Phone, @Email, @MembershipDate)";

        await con.ExecuteAsync(sql, member);
    }

   public async Task DeleteMember(int id)
{
    using var con = context.GetConnection();

    var sql1 = "delete from borrowings where memberid = @Id";
    var sql2 = "delete from members where memberid = @Id";

    await con.ExecuteAsync(sql1, new { Id = id });
    await con.ExecuteAsync(sql2, new { Id = id });
}

    public async Task<List<Member>> GetAllMembers()
    {
        using var con = context.GetConnection();

        var sql = "select * from members";

          var mb = con.Query<Member>(sql).ToList();
        
         return mb;
    }

    public async Task<Member> GetMemberById(int id)
    {
        using var con = context.GetConnection();

        var sql = "select * from members where memberid = @Id";

        var mb = await con.QuerySingleOrDefaultAsync<Member>(sql, new {Id = id});
        return mb;
    }

    public async Task UpdateMember(Member member)
    {
        using var con = context.GetConnection();

        var sql = @"update members
                    set fullname = @FullName,
                     phone = @Phone,
                     email = @Email,
                     membershipdate = @MembershipDate
                    where memberid = @MemberId";

        await con.ExecuteAsync(sql, member);
    }

    public async Task<dynamic> GetMostActiveMember()
{
    using var con = context.GetConnection();

    var sql = @"select m.fullname, count(b.borrowingid) as total
                from members m
                join borrowings b on m.memberid = b.memberid
                group by m.fullname
                order by total desc
                limit 1";

    var mb = await con.QuerySingleOrDefaultAsync(sql);

    return mb;
}

public async Task<dynamic> GetFirstLateMember()
{
    using var con = context.GetConnection();

    var sql = @"select m.fullname
                from members m
                join borrowings br on m.memberid = br.memberid
                where br.returndate > br.duedate
                limit 1";

    var mr = await con.QueryFirstOrDefaultAsync(sql);

    return mr;
}

  
}