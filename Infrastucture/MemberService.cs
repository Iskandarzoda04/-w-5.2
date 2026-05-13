using Domain;
using Dapper;
using Infrastructure.Interfacee;
using Infrastructure.Date;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class MemberService : IMemberService
{
    private readonly DateContext _context;
    private readonly ILogger<MemberService> _logger;

    public MemberService(DateContext context, ILogger<MemberService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddMember(Member member)
    {
        _logger.LogInformation("Adding a new member: FullName={FullName}, Phone={Phone}", 
            member.FullName, member.Phone);
        try
        {
            using var con = _context.GetConnection();
            var sql = @"insert into members(fullname, phone, email, membershipdate)
                        values(@FullName, @Phone, @Email, @MembershipDate)";
            await con.ExecuteAsync(sql, member);
            _logger.LogInformation("Member added successfully: FullName={FullName}", member.FullName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding member: FullName={FullName}", member.FullName);
            throw;
        }
    }

    public async Task DeleteMember(int id)
    {
        _logger.LogInformation("Deleting member: Id={Id}", id);
        try
        {
            using var con = _context.GetConnection();
            var sql1 = "delete from borrowings where memberid = @Id";
            var sql2 = "delete from members where memberid = @Id";
            await con.ExecuteAsync(sql1, new { Id = id });
            await con.ExecuteAsync(sql2, new { Id = id });
            _logger.LogInformation("Member deleted successfully: Id={Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting member: Id={Id}", id);
            throw;
        }
    }

    public async Task<List<Member>> GetAllMembers()
    {
        _logger.LogInformation("GetAllMembers started");
        try
        {
            using var con = _context.GetConnection();
            var sql = "select * from members";
            var mb = con.Query<Member>(sql).ToList();
            _logger.LogInformation("GetAllMembers: {Count} members found", mb.Count);
            return mb;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving all members");
            throw;
        }
    }

    public async Task<Member> GetMemberById(int id)
    {
        _logger.LogInformation("GetMemberById started: Id={Id}", id);
        try
        {
            using var con = _context.GetConnection();

            var sql = "select * from members where memberid = @Id";

            var mb = await con.QuerySingleOrDefaultAsync<Member>(sql, new { Id = id });
            
            if (mb == null)
                _logger.LogWarning("Member not found: Id={Id}", id);
            else
                _logger.LogInformation("Member found: Id={Id}, FullName={FullName}", id, mb.FullName);
            return mb;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving member: Id={Id}", id);
            throw;
        }
    }

    public async Task UpdateMember(Member member)
    {
        _logger.LogInformation("Updating member: Id={Id}, FullName={FullName}", 
            member.MemberId, member.FullName);
        try
        {
            using var con = _context.GetConnection();
            var sql = @"update members
                        set fullname = @FullName,
                            phone = @Phone,
                            email = @Email,
                            membershipdate = @MembershipDate
                        where memberid = @MemberId";

            await con.ExecuteAsync(sql, member);

            _logger.LogInformation("Member updated successfully: Id={Id}, FullName={FullName}", 
                member.MemberId, member.FullName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating member: Id={Id}", member.MemberId);
            throw;
        }
    }

    public async Task<dynamic> GetMostActiveMember()
    {
        _logger.LogInformation("GetMostActiveMember started");
        try
        {
            using var con = _context.GetConnection();
            var sql = @"select m.fullname, count(b.borrowingid) as total
                        from members m
                        join borrowings b on m.memberid = b.memberid
                        group by m.fullname
                        order by total desc
                        limit 1";
            var mb = await con.QuerySingleOrDefaultAsync(sql);

            if (mb == null)
                _logger.LogWarning("Most active member not found");
            else
                _logger.LogInformation("Most active member found: FullName={FullName}", (string)mb.fullname);
            return mb;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving most active member");
            throw;
        }
    }

    public async Task<dynamic> GetFirstLateMember()
    {
        _logger.LogInformation("GetFirstLateMember started");
        try
        {
            using var con = _context.GetConnection();
            var sql = @"select m.fullname
                        from members m
                        join borrowings br on m.memberid = br.memberid
                        where br.returndate > br.duedate
                        limit 1";
            var mr = await con.QueryFirstOrDefaultAsync(sql);

            if (mr == null)
                _logger.LogWarning("Late member not found");
            else
                _logger.LogInformation("First late member found: FullName={FullName}", (string)mr.fullname);
            return mr;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving first late member");
            throw;
        }
    }
}