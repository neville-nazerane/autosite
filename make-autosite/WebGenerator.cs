using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace make_autosite
{
    class WebGenerator
    {
        private readonly string name;

        public WebGenerator(string name)
        {
            this.name = name;
        }

        const string dbContextName = "ApplicationDbContext";

        internal async Task GenerateAsync()
        {
            var res = await AutoConsumer.GetSiteAsync(name);
            if (res.IsSuccessful)
            {
                AutoConsumer.ShowBase();
                var content = res.Data;
                string siteName = name.Replace(" ", "_");

                string unique = Guid.NewGuid().ToString();

                RunDotnet($"new mvc", "Creating new website");
                RunDotnet("add package Microsoft.VisualStudio.Web.CodeGeneration.Design", "Setting things up");
                RunDotnet("restore");

                string projectFile =
                        Directory.GetFiles(Directory.GetCurrentDirectory())
                                    .SingleOrDefault(f => f.EndsWith(".csproj"));
                string projectName = projectFile.Split("\\").Last().Replace(".csproj", "").Replace(" ", "_");
                Info("using project: " + projectName);

                Info("Setting up models");

                var menuList = content.ClassItems.Select(c => {
                    string className = c.Name.CamelCase();
                    File.WriteAllText($"Models/{className}.cs", new ClassBuilder(c, projectName).Build());
                    RunDotnet("aspnet-codegenerator controller " + ControllerGeneratorOptions(className, projectName));
                    return $"\n<li class='nav-item'><a class='nav-link text-dark' href='~/{className}'>{className}</a>";
                });
                string menuItems = string.Join("</li>", menuList);
                string layout = "Views/Shared/_Layout.cshtml";
                string menuBefore = ">Privacy</a>";
                File.WriteAllText(layout, File.ReadAllText(layout).Replace(menuBefore,
                                            menuBefore + "</li>" + menuItems));

                Info("Creating database");
                string startup = "Startup.cs";
                File.WriteAllText(startup, File.ReadAllText(startup).Replace(
                        "UseSqlServer(Configuration.GetConnectionString(\"ApplicationDbContext\"))",
                        "UseSqlite(\"Data Source = database.db\")"
                    ));
                RunDotnet("add package Microsoft.EntityFrameworkCore.Sqlite");
                RunDotnet("restore");
                RunDotnet("ef migrations add AutoSiteInit");
                RunDotnet("ef database update");
                RunDotnet("run");
            }
            else
            {
                if (res.StatusCode == HttpStatusCode.NotFound)
                    Console.WriteLine("Site name not found");
                else
                    Console.WriteLine($"Found error: {res.StatusCode}, with message '{res.TextResponse}'");
            }
        }

        string ControllerGeneratorOptions(string model, string projectName)
            => $"-name {model}Controller -m {model} -dc {dbContextName} -l _Layout -namespace {projectName}.Controllers -outDir Controllers";

        void RunDotnet(string command, string message = "") => RunCLI("dotnet.exe", command, message);

        void RunCLI(string file, string command, string message = "")
        {
            Info(message);
            var process = Process.Start(file, command);
            process.WaitForExit();
        }

        void Info(string message)
        {
            var col = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + message + "\n");
            Console.ForegroundColor = col;
        }

        void Error(string message)
        {
            var col = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + message + "\n");
            Console.ForegroundColor = col;
        }

    }
}
