using AutoSite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DbContextExtesions
    {

        const string ConnectionStringKey = "sqlServer";
        //const string AssemblyName = "AutoSite.Migrations";

        public static void ConfigureDb(
                                        this DbContextOptionsBuilder config, 
                                        IConfiguration configuration,
                                        string assemblyName = null
                                ) =>
            config.UseSqlServer(configuration.GetConnectionString(ConnectionStringKey),
                             b => {
                                 if (assemblyName != null) b.MigrationsAssembly(assemblyName);
                                 });

    }
}
