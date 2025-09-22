using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuestiaCodingTask.Helpers;
using Microsoft.Data.SqlClient;

namespace GuestiaCodingTask.Data.Repositories
{
    public class AdoGuestRepository : IAdoGuestRepository
    {
        private const string _connectionString = @"Server=localhost, 1433;Database=GuestiaDB;User Id=sa;Password=P@ssw0rd!23;TrustServerCertificate=true;";

        public async Task<Dictionary<string, List<Guest>>> GetAllUnregisteredGroupedAsync()
        {
            var guests = new List<Guest>();

            const string sql = @"
                select g.Id, g.FirstName, g.LastName, g.RegistrationDate, gg.Id, gg.Name as GroupName
                from Guests g
                inner join GuestGroups gg
                on gg.Id = g.GuestGroupId";

            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if(reader.HasRows)
                    {
                        var guest = new Guest
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            RegistrationDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                            GuestGroup = new GuestGroup
                            {
                                Id = reader.GetInt32(4),
                                Name = reader.GetString(5)
                            }
                        };
                                
                        guests.Add(guest);
                    }
                }                       
            }
            
            // Group and sort the guests
            var groupedGuests = guests
                .GroupBy(g => g.GuestGroup.Name)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .OrderBy(guest => NameDisplayFormatTypeHelper.FormatGuestName(guest))
                        .ToList());

            return groupedGuests;            
        }
    }

    public interface IAdoGuestRepository
    {
        Task<Dictionary<string, List<Guest>>> GetAllUnregisteredGroupedAsync();
    }
}

/*
 * T-SQL Query
 
 SELECT 
       gg.Name as GroupName,
       CASE 
           WHEN gg.NameDisplayFormat = 1 
               THEN UPPER(g.LastName) + ' ' + g.FirstName
           WHEN gg.NameDisplayFormat = 2 
               THEN g.LastName + ', ' + UPPER(LEFT(g.FirstName, 1))
           ELSE 
               UPPER(g.LastName) + ' ' + g.FirstName
       END AS FormattedName
   FROM Guests g
   INNER JOIN GuestGroups gg ON gg.Id = g.GuestGroupId
   WHERE g.RegistrationDate IS NULL
   ORDER BY 
       gg.Name DESC,
       CASE 
           WHEN gg.NameDisplayFormat = 1 
               THEN UPPER(g.LastName) + ' ' + g.FirstName
           WHEN gg.NameDisplayFormat = 2 
               THEN g.LastName + ', ' + UPPER(LEFT(g.FirstName, 1))
           ELSE 
               UPPER(g.LastName) + ' ' + g.FirstName
       END ASC
*/