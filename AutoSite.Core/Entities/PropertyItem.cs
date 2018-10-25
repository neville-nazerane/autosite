using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutoSite.Core.Entities
{
    public class PropertyItem
    {

        public int Id { get; set; }

        public int ClassItemId { get; set; }
        public ClassItem ClassItem { get; set; }

        [Required, MaxLength(300)]
        public string Name { get; set; }

        public PropertyTypes DataType { get; set; }
    }
}
