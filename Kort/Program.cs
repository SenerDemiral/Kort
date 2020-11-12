using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Kort
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                   webBuilder.UseStartup<Startup>();
                   //webBuilder.UseStartup<Startup>().UseUrls(new[] { "http://localhost:5002" }); // now the Kestrel server will listen on port 5002!
                });
    }
}
