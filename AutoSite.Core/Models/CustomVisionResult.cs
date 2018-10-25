using System;
using System.Collections.Generic;
using System.Text;

namespace AutoSite.Core.Models
{
    public class CustomVisionResult
    {

        public Prediction[] Predictions { get; set; }

        public class Prediction
        {
            public double Probability { get; set; }

            public string TagName { get; set; }

        }

    }
}
