
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;


namespace DAL
{
    public class GisContext : DbContext
    {
        public GisContext() : base("DBConnection")
        {
                
        }

        public DbSet<Road> Roads { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Road>().Property(x => x.Length).HasColumnType("numeric");

        }


    }

}
