using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuestiaCodingTask.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GuestiaCodingTask.Data.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private readonly GuestiaContext _context;

        public GuestRepository(GuestiaContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, List<Guest>>> GetAllUnregisteredGroupedAsync()
        {
            var unregisteredGuests = await _context.Guests
                .Include(g => g.GuestGroup)
                .Where(g => g.RegistrationDate == null)
                .ToListAsync();

            var groupedGuests = unregisteredGuests
                .GroupBy(g => g.GuestGroup.Name)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .OrderBy(guest => NameDisplayFormatTypeHelper.FormatGuestName(guest))
                        .ToList());
            
            return groupedGuests;
        }
    }

    public interface IGuestRepository
    {
        Task<Dictionary<string, List<Guest>>> GetAllUnregisteredGroupedAsync();
    }
}