using Hangfire;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Reminder.Services
{
    public class HangfireService
    {
        private readonly IBackgroundJobClient jobClient;

        public HangfireService(IBackgroundJobClient jobClient)
        {
            this.jobClient = jobClient;
        }

        public void Schedule(TimeSpan time)
        {
            jobClient.Schedule(() => Console.WriteLine($"Exec {time} lepta"), time);
        }
    }
}
