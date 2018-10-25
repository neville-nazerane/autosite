using AutoSite.Core.Entities;
using AutoSite.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoSite.Services
{
    public interface ISiteContentRepository
    {
        SiteContent Get(int id);
        ClassItem Add(ClassItem item);
        PropertyItem Add(PropertyItem item);
        Task<PropertyItem> AddAsync(PropertyItem item);
        IEnumerable<SiteContent> Get();
        SiteContent Add(SiteContent site);
        void DeleteClass(int id);
        void DeleteProperty(int id);
        int GetIdFromPropertyItem(int id);
        int GetIdFromClassItem(int id);
        ClassItem GetClassItem(int id);
        SiteContent GetByName(string name);
        ClassItem Update(ClassItem item);
        PropertyItem GetPropertyItem(int id);
        PropertyItem Update(PropertyItem item);
        Task ImportAsync(ImportClassItem item);
        void Delete(int id);
        SiteContent Update(SiteContent content);
    }
}
