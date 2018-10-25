using AutoSite.Core.Entities;
using AutoSite.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCore.ModelValidation.Core;
using AutoSite.Core.Models;

namespace AutoSite.Business
{
    partial class SiteContentRepository : ISiteContentRepository
    {
        private readonly AppDbContext context;
        private readonly ModelValidator modelValidator;
        private readonly ITypeSuggester suggester;
        private readonly IEntityUnderstanding entityUnderstanding;
        private readonly IImageReader reader;
        private readonly ICustomVisionReader customVisionReader;

        public SiteContentRepository(
                                        AppDbContext context, 
                                        ModelValidator modelValidator,
                                        ITypeSuggester suggester, 
                                        IEntityUnderstanding entityUnderstanding,
                                        IImageReader reader,
                                        ICustomVisionReader customVisionReader
            )
        {
            this.context = context;
            this.modelValidator = modelValidator;
            this.suggester = suggester;
            this.entityUnderstanding = entityUnderstanding;
            this.reader = reader;
            this.customVisionReader = customVisionReader;
        }

        public ClassItem Add(ClassItem item)
        {
            if (context.ClassItems.Any(c => item.SiteContentId == c.SiteContentId &&
                                                c.Name.Equals(item.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                modelValidator.AddError("Class with this name already exists");
                return null;
            }
            context.Add(item);
            context.SaveChanges();
            return item;
        }

        public PropertyItem Add(PropertyItem item)
        {
            if (context.PropertyItems.Any(c => c.ClassItemId == item.ClassItemId &&
                            c.Name.Equals(item.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                modelValidator.AddError("Property with this name already exists");
                return null;
            }
            context.Add(item);
            context.SaveChanges();
            return item;
        }

        public SiteContent Add(SiteContent site)
        {
            if (context.SiteContent.Any(c => c.Name.Equals(site.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                modelValidator.AddError("Site with this name already exists");
                return null;
            }
            context.Add(site);
            context.SaveChanges();
            return site;
        }

        public void Delete(int id)
        {
            context.Remove(context.SiteContent.Find(id));
            context.SaveChanges();
        }

        public void DeleteClass(int id)
        {
            context.Remove(context.ClassItems.Find(id));
            context.SaveChanges();
        }

        public void DeleteProperty(int id)
        {
            context.Remove(context.PropertyItems.Find(id));
            context.SaveChanges();
        }

        public SiteContent Get(int id) => context.SiteContent.AsNoTracking()
                                                .Include(s => s.ClassItems)
                                                .ThenInclude(c => c.Properties)
                                            .SingleOrDefault(s => s.Id == id);

        public IEnumerable<SiteContent> Get() => context.SiteContent.AsNoTracking();

        public SiteContent GetByName(string name) => context.SiteContent.AsNoTracking()
                                                        .Include(s => s.ClassItems)
                                                        .ThenInclude(c => c.Properties)
                                                    .SingleOrDefault(s => s.Name == name);

        public ClassItem GetClassItem(int id) => context.ClassItems.Find(id);

        public int GetIdFromClassItem(int id)
            => (from c in context.ClassItems
                where c.Id == id
                select c.SiteContentId).SingleOrDefault();

        public int GetIdFromPropertyItem(int id)
            => (from p in context.PropertyItems
                where p.Id == id
                select p.ClassItem.SiteContentId).SingleOrDefault();

        public PropertyItem GetPropertyItem(int id) => context.PropertyItems.Find(id);

        public ClassItem Update(ClassItem item)
        {
            if (context.ClassItems.Any(c => item.Id != c.Id && item.SiteContentId == c.SiteContentId &&
                                    c.Name.Equals(item.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                modelValidator.AddError("Class with this name already exists");
                return null;
            }
            context.Update(item);
            context.SaveChanges();
            return item;
        }

        public PropertyItem Update(PropertyItem item)
        {
            if (context.PropertyItems.Any(c => c.ClassItemId == item.ClassItemId &&
                c.Name.Equals(item.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                modelValidator.AddError("Property with this name already exists");
                return null;
            }
            context.Update(item);
            context.SaveChanges();
            return item;
        }

        public SiteContent Update(SiteContent site)
        {
            if (context.SiteContent.Any(c => c.Id != site.Id && c.Name.Equals(site.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                modelValidator.AddError("Site with this name already exists");
                return null;
            }
            context.Update(site);
            context.SaveChanges();
            return site;
        }
    }
}
