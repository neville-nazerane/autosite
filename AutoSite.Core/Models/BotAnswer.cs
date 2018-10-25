using System;
using System.Collections.Generic;
using System.Text;

namespace AutoSite.Core.Models
{
    public class BotAnswer
    {

        public int Id { get; set; }

        public string[] Questions { get; set; }

        public string Answer { get; set; }

        public double Score { get; set; }

    }
}
