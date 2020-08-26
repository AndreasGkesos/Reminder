using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;


namespace Reminder
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    IServiceCollection services = new ServiceCollection();
        //    // Startup.cs finally :)
        //    Startup startup = new Startup();
        //    startup.ConfigureServices(services);
        //    IServiceProvider serviceProvider = services.BuildServiceProvider();
        //}

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .UseStartup<Startup>();
    }
}
