using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace PharmaStoreAPI.Core.Models
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PharmaStoreContext>
    {
        PharmaStoreContext IDesignTimeDbContextFactory<PharmaStoreContext>.CreateDbContext(string[] args)
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<PharmaStoreContext>();
            var connectionString = configuration.GetConnectionString("Database");
            builder.UseSqlServer(connectionString);
            return new PharmaStoreContext(builder.Options);
        }
    }
}
