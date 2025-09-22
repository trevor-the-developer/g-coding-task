using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuestiaCodingTask.Data;
using GuestiaCodingTask.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GuestiaCodingTask.Test.RepositoryTests
{
    public class AdoGuestRepsitoryTests : IDisposable
    {
        private readonly GuestiaContext _context;
        private readonly IGuestRepository _guestRepository;

        public AdoGuestRepsitoryTests()
        {
            var options = new DbContextOptionsBuilder<GuestiaContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new GuestiaContext(options);
            _guestRepository = new GuestRepository(_context);
            
            // seed the database
            TestData.SeedData(_context);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
        
        [Fact]
        public async Task GetAllUnregisteredGroupedAsync_Returns_All_GroupedGuests()
        {
            // Arrange
    
            // Act
            var results = await _guestRepository.GetAllUnregisteredGroupedAsync();

            // Assert
            Assert.NotNull(results);
            Assert.IsType<Dictionary<string, List<Guest>>>(results);
            Assert.True(results.Count > 0, "Should have at least one group");
            var allGuestsInResult = results.Values.SelectMany(guests => guests);
            Assert.True(allGuestsInResult.All(
                    guest => guest.RegistrationDate == null), 
                "All guests should be unregistered"
            );
        }
    }
}