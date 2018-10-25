using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutoSite.Core.Entities
{
    public class ClassItem
    {

        public int Id { get; set; }

        [Required, MaxLength(300)]
        public string Name { get; set; }

        public int SiteContentId { get; set; }

        public IEnumerable<PropertyItem> Properties { get; set; }

    }
}
