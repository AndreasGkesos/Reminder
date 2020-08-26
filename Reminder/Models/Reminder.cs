using System;
using System.Collections.Generic;
using System.Text;

namespace Reminder.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime When { get; set; }
    }
}
