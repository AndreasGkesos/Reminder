using Microsoft.AspNetCore.Mvc;
using Reminder.Models;
using Reminder.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reminder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly ReminderDbContext context;
        private readonly EmailService emailService;
        private readonly HangfireService hangfireService;

        public ReminderController(ReminderDbContext context, EmailService emailService, HangfireService hangfireService)
        {
            this.context = context;
            this.emailService = emailService;
            this.hangfireService = hangfireService;
        }

        [HttpGet]
        [Route("Run")]
        public int Run()
        {
            var reminder = this.context.Reminders.Find(3);
            // this.emailService.Send("a.gkesos@codehub.gr", "a.gkesos@codehub.gr", "asdf", "The best email ever!");
            var hours = (reminder.When - DateTime.UtcNow).Hours;

            hangfireService.Schedule(new TimeSpan(hours: 0, minutes: 1, seconds: 0));
            hangfireService.Schedule(new TimeSpan(hours: 0, minutes: 2, seconds: 0));

            // schedule hours - 1
            // hangfireService.Schedule(new TimeSpan(hours: hours - 1, minutes: 0, seconds: 0));
            // if hours > 15 then schedule hours - 15
            //if ((hours - 15) > 15) hangfireService.Schedule(new TimeSpan(hours: hours - 15, minutes: 0, seconds: 0));

            return 1;
        }
    }
}
