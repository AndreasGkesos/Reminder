using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reminder.Models;
using Hangfire;
using Hangfire.SqlServer;
using System;
using Reminder.Infrastructure;
using Reminder.Services;

namespace Reminder
{    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(Configuration.GetConnectionString("default"), new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));
            services.AddHangfire(configuration => configuration
                    .UseSqlServerStorage(Configuration.GetConnectionString("default")));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddDbContext<ReminderDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("default")));

            services.AddTransient<EmailService>();
            services.AddTransient<HangfireService>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs, IServiceProvider serviceProvider)
        {
            GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(serviceProvider));
            app.UseHangfireDashboard();
            // backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard("/hangfire");
            });
        }
    }
}
