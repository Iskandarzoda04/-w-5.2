using System.Data;
using Npgsql;
namespace Infrastructure.Date;

public class DateContext
{
    public readonly string connectionString = "Server=localhost;Database=Exxam;User Id=postgres;Password=12345";
   


     public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(connectionString);
    }
        
}



