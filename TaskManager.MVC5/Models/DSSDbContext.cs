using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TaskManager.MVC5.Models
{
    public class DSSDbContext : DbContext
    {
        
        public DSSDbContext() : base("connString")
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<attendance> attendances { get; set; }
        public DbSet<Taskss>Tasks { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Skill> Skills { get; set; }
       

       
        public System.Data.Entity.DbSet<TaskManager.MVC5.Models.EmployeeViewModel> EmployeeViewModels { get; set; }
    }
}