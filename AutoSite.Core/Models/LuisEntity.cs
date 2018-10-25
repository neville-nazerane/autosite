using System;
using System.Collections.Generic;
using System.Text;

namespace AutoSite.Core.Models
{
    public class LuisEntity
    {

        public string Entity { get; set; }

        public string Type { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public EntityResolution Resolution { get; set; }

        public class EntityResolution {

            public string Subtype { get; set; }

        }

    }
}
