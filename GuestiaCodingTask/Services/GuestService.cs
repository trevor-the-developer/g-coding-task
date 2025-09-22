using System.Collections.Generic;
using System.Threading.Tasks;
using GuestiaCodingTask.Data;
using GuestiaCodingTask.Data.Repositories;

namespace GuestiaCodingTask.Services
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _guestRepository;

        public GuestService(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }


        public async Task<Dictionary<string, List<Guest>>> GetAllUnregisteredGroupedAsync()
        {
            var guests = await _guestRepository.GetAllUnregisteredGroupedAsync();
            return guests;
        }
    }

    public interface IGuestService
    {
        Task<Dictionary<string, List<Guest>>> GetAllUnregisteredGroupedAsync();
    }
}