using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AutoSite.Core.Entities;
using AutoSite.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoSite.Migrations.Data
{
    class MigrationDbContext : AbstractDbContext
    {
        public MigrationDbContext(DbContextOptions<MigrationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetUnique<SiteContent>(s => s.Name)
                        .SetUnique<ClassItem>(c => new { c.Name, c.SiteContentId })
                        .SetUnique<PropertyItem>(p => new { p.Name, p.ClassItemId });
            base.OnModelCreating(modelBuilder);
        }

    }

    static class ContextExtensions
    {

        public static ModelBuilder SetUnique<TEntity>(this ModelBuilder modelBuilder,
                        Expression<Func<TEntity, object>> indexExpression)
             where TEntity : class
        {
            modelBuilder.Entity<TEntity>().HasIndex(indexExpression).IsUnique();
            return modelBuilder;
        }

    }

}
