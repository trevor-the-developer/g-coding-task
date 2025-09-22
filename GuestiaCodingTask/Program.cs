using GuestiaCodingTask.Data;
using GuestiaCodingTask.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuestiaCodingTask.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GuestiaCodingTask
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        
        static async Task Main(string[] args)
        {
            DbInitialiser.CreateDb();

            // setup the services
            _serviceProvider = ServiceContainer.ConfigureServices();

            // get the data
            var guestService = _serviceProvider.GetRequiredService<IGuestService>();
            var unregisteredGroupedGuests = await guestService.GetAllUnregisteredGroupedAsync();

            // display the report
            DisplayUnregisteredGroupedGuests(unregisteredGroupedGuests);
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void DisplayUnregisteredGroupedGuests(Dictionary<string,List<Guest>> unregisteredGroupedGuests)
        {
            Console.WriteLine("Unregistered guests report:");
            Console.WriteLine("---------------------------");
            Console.WriteLine();

            foreach (var group in unregisteredGroupedGuests)
            {
                Console.WriteLine("");
                Console.WriteLine($"{group.Key} Group:");
                Console.WriteLine("");

                foreach (var guest in group.Value)
                {
                    var formattedGuestName = NameDisplayFormatTypeHelper.FormatGuestName(guest);
                    Console.WriteLine($"{formattedGuestName}");
                }
            }
        }
    }
}
