
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Everyday.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> user { get; set; }
        public DbSet<Company> company { get; set; }
        public DbSet<Resume> resume { get; set; }
        public DbSet<Vacancy> vacancy { get; set; }
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;UserId=root;Password=12345678;database=qwork;");
        }
    }
}