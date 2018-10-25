using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoSite.Website
{
    public class SwaggerFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths;
            var remove = paths.Where(p => !p.Key.StartsWith("/api/")).ToList();
            foreach (var p in remove)
                paths.Remove(p);
        }
    }
}
