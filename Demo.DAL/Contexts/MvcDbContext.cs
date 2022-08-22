using Demo.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Contexts
{
    public class MvcDbContext : IdentityDbContext<AppUser>
    {

        public MvcDbContext(DbContextOptions<MvcDbContext> options): base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseSqlServer(@"Server = DESKTOP-IP35PQM\MSSQLSERVER02;Database = MvcDb; Trusted_connection = true; MultipleActiveResultSets = true;");


        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
