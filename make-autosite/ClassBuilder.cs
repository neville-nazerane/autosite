using AutoSite.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace make_autosite
{
    class ClassBuilder
    {
        private readonly ClassItem contents;

        public string ProjectName { get; set; }

        internal ClassBuilder(ClassItem contents, string projectName)
        {
            this.contents = contents;
            ProjectName = projectName;
        }

        internal string Build()
        {
            var properties = contents.Properties.Union(new PropertyItem[] { new PropertyItem { Name = "Id", DataType = PropertyTypes.integer } });
            string output = $@"
using System;
using System.ComponentModel.DataAnnotations;

namespace {ProjectName}.Models
{{
    public class {contents.Name.CamelCase()} 
    {{
        {string.Join("\n    ", properties.Select(p => Make(p)))}

    }}
}}
            ";

            return output;
        }

        string Make(PropertyItem item)
        {
            string name = item.Name.CamelCase();

            var annotations = new List<string>();
            string type = "string";
            switch (item.DataType)
            {
                case PropertyTypes.@decimal:
                    type = "double";
                    break;
                case PropertyTypes.age:
                    type = "int";
                    annotations.Add("Range(0, 200)");
                    break;
                case PropertyTypes.name:
                    annotations.Add("MaxLength(40, ErrorMessage = \"Name needs to be below 40 letters\")");
                    break;
                case PropertyTypes.cost:
                    type = "int";
                    break;
                case PropertyTypes.url:
                    annotations.Add("Url");
                    break;
                case PropertyTypes.email:
                    annotations.Add("EmailAddress");
                    break;
                case PropertyTypes.phone:
                    annotations.Add("Phone");
                    break;
                case PropertyTypes.integer:
                    type = "int";
                    break;
                case PropertyTypes.time:
                    type = "DateTime";
                    annotations.Add("DataType(DataType.Time)");
                    break;
                case PropertyTypes.date:
                    type = "DateTime";
                    annotations.Add("DataType(DataType.Date)");
                    break;
                case PropertyTypes.dateTime:
                    type = "DateTime";
                    break;
            }
            if (name != item.Name)
                annotations.Add($"Display(Name = \"{item.Name}\")");

            string annotationStr = string.Empty;
            if (annotations.Count > 0)
                annotationStr = "\n" + string.Join("\n", annotations.Select(a => $"          [{a}]")) + "\n          ";
            return $"{annotationStr}public {type} {name} {{ get; set; }}";
        }
    }
}
