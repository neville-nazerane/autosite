using AutoSite.Core.Entities;
using AutoSite.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSite.Business
{
    partial class SiteContentRepository
    {

        public async Task ImportAsync(ImportClassItem item)
        {
            string type = await customVisionReader.GetPrediction(item.Image);
            var lines = await reader.GetTextAsync(item.Image, type == "table");
            foreach (var line in lines)
                await AddAsync(new PropertyItem { ClassItemId = item.ClassId, Name = line });
        }

        public async Task<PropertyItem> AddAsync(PropertyItem item) 
            => await AddAsync(item, null);

        async Task<PropertyItem> AddAsync(PropertyItem item, Dictionary<string, int> pastNames)
        {
            string title = null;
            string text = item.Name;
            var vals = text.Split(new string[] { ":|:" }, StringSplitOptions.None);
            if (vals.Length == 2)
            {
                title = vals[0];
                text = vals[1];
            }
            var understanding = await entityUnderstanding.GetAsync(text);

            if (understanding.TopScoringIntent?.Intent != "None")
            {
                string intent = understanding.TopScoringIntent?.Intent;
                if (intent.Contains("Address")) item.DataType = PropertyTypes.text;
                item.Name = intent;
            } 
            else
            {
                var entities = understanding.Entities;
                var lasts = entities.Where (
                        e => e.EndIndex == entities.Max(m => m.EndIndex)
                    );
                LuisEntity entity = null;
                // getting the best possible match from luis
                if (lasts.Count() == 1) entity = lasts.SingleOrDefault();
                else if (lasts.Count() > 1)
                    entity = lasts.FirstOrDefault(e => e.StartIndex == lasts.Min(m => m.StartIndex));
            
                if (entity != null && entity.StartIndex > 0)
                {
                    MakeItemFromEntity(entity, item);
                    // initial part taken as property name
                    if (title == null)
                        item.Name = item.Name.Substring(0, entity.StartIndex);
                    else item.Name = title;
                }
                else
                {
                    // if start index is 0, still double check with QnA
                    string result = await suggester.SuggestAsync(text);
                    if (Enum.TryParse(result, out PropertyTypes type))
                        item.DataType = type;
                    else
                    {
                        // if nothing in QnA use LUIS result
                        if (entity != null)
                        {
                            MakeItemFromEntity(entity, item);
                            // if complete name string is the entity, Name needs to be generated
                            NameFromEntity(entity.Type, item, pastNames);
                        }
                        // nothing in LUIS either then nothing
                        else item.DataType = PropertyTypes.undetected;
                    }
                }
            }
            if (title != null) item.Name = title;
            return Add(item);
        }

        static void MakeItemFromEntity(LuisEntity entity, PropertyItem item)
        {
            
            switch (entity.Type)
            {
                case "builtin.age":
                    item.DataType = PropertyTypes.age;
                    break;
                case "builtin.number":
                    if (entity.Resolution?.Subtype == "integer")
                        item.DataType = PropertyTypes.@decimal;
                    else item.DataType = PropertyTypes.integer;
                    break;
                case "builtin.datetimeV2.date":
                    item.DataType = PropertyTypes.date;
                    break;
                case "builtin.datetimeV2.time":
                    item.DataType = PropertyTypes.time;
                    break;
                case "builtin.datetimeV2.datetime":
                    item.DataType = PropertyTypes.dateTime;
                    break;
                case "builtin.url":
                    item.DataType = PropertyTypes.url;
                    break;
                case "builtin.email":
                    item.DataType = PropertyTypes.email;
                    break;
                case "builtin.currency":
                    item.DataType = PropertyTypes.cost;
                    break;
                case "builtin.personName":
                    item.DataType = PropertyTypes.name;
                    break;
                case "builtin.phonenumber":
                    item.DataType = PropertyTypes.phone;
                    break;
                default:
                    item.DataType = PropertyTypes.undetected;
                    break;
            }
        }

        void NameFromEntity(string entityType, PropertyItem item, Dictionary<string, int> pastNames)
        {
            string result = entityType.Split('.').Last();
            if (pastNames != null)
            {
                if (pastNames.TryGetValue(result, out int count))
                {
                    pastNames[result] = ++count;
                    result += count;
                }
                else pastNames[result] = 1;
            }

            var existing = (from p in context.PropertyItems
                            where p.ClassItemId == item.ClassItemId && p.Name.StartsWith(result, StringComparison.InvariantCultureIgnoreCase)
                            select p.Name).ToArray();
            string val = result;
            int i = 1;
            while (existing.Any(e => e.Equals(result, StringComparison.InvariantCultureIgnoreCase)))                              
                result = val + ++i;
            item.Name = result;
        }

    }
}
