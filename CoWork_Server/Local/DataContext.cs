using System;
using System.IO;
using Co_Work.Core.Employee;
using Co_Work.Core.Project;
using Microsoft.EntityFrameworkCore;

namespace Co_Work.Local
{
    public class DataContext:DbContext
    {
        public DbSet<Project> ProjectDataBase { get; set; }
        public DbSet<Employee> EmployeeDataBase { get; set; }
        public DbSet<Account> AccountDataBase { get; set; }
        
        public DbSet<PastProject> PastProjectsDataBase { get; set; }
        public string ConnectingString { get; set; }
        
        public DataContext(string dbFile)
        {
            ConnectingString = "Data Source="+Path.Combine(Environment.CurrentDirectory,dbFile);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectingString);
        }

        
    }
}