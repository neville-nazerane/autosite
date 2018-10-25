using AutoSite.Core.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoSite.Data
{
    public abstract class AbstractDbContext : DbContext
    {
        public AbstractDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<SiteContent> SiteContent { get; set; }

        public DbSet<ClassItem> ClassItems { get; set; }

        public DbSet<PropertyItem> PropertyItems { get; set; }
        

    }
}
