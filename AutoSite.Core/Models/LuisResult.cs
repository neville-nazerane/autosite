using System;
using System.Collections.Generic;
using System.Text;

namespace AutoSite.Core.Models
{
    public class LuisResult
    {

        public string Query { get; set; }

        public LuisIntent TopScoringIntent { get; set; }

        public LuisEntity[] Entities { get; set; }

    }
}
