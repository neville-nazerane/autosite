using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutoSite.Core.Entities
{
    public class SiteContent
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<ClassItem> ClassItems { get; set; }

    }
}
