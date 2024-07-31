using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;

namespace CongratulationsApp
{
    internal class ApplicationDB : DbContext
    {
        public DbSet<Context> data { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=LocalHost; Database=birthdays; Username=postgres; Password=1111");
        }
    }

}
