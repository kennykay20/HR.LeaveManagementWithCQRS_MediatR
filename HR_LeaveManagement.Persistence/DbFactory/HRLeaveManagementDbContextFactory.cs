using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Persistence.DbFactory
{
    public class HRLeaveManagementDbContextFactory
        : IDesignTimeDbContextFactory<HRLeaveManagementDbContext>
    {
        public HRLeaveManagementDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            //var basePath = Path.Combine(
            //                    Directory.GetCurrentDirectory(),
            //                    "../HR_LeaveManagement.Api");

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HRLeaveManagementDbContext>();

            optionsBuilder.UseSqlServer(config.GetConnectionString("HRLeaveConnectionString"));

            return new HRLeaveManagementDbContext(optionsBuilder.Options);
        }
    }
}
