using CodeChallenge.Config;
using CodeChallenge.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace CodeChallenge
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var app = new App().Configure(args);

            // startup events here
            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                await scope.ServiceProvider.GetService<EmployeeDataSeeder>().Seed();
            }

            await app.RunAsync();
        }
    }
}