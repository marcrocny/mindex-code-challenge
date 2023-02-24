using CodeChallenge.Config;
using CodeChallenge.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace CodeChallenge
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var app = new App().Configure(args);

            // asynchronous startup events here
            if (app.Environment.IsDevelopment())
                await SeedEmployeeDB();

            await app.RunAsync();
        }

        /// <summary>Add seed data to the in-memory DB.</summary>
        /// <remarks>Moved from app-config so that it can be properly awaited at startup.</remarks>
        private static Task SeedEmployeeDB()
            => new EmployeeDataSeeder(
                new EmployeeContext(
                    new DbContextOptionsBuilder<EmployeeContext>().UseInMemoryDatabase("EmployeeDB").Options
            )).Seed();
    }
}