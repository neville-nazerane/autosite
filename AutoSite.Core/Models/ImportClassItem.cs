using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutoSite.Core.Models
{
    public class ImportClassItem
    {

        public int ClassId { get; set; }

        [Required]
        public IFormFile Image { get; set; }

    }
}
