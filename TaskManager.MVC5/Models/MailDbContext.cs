using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TaskManager.MVC5.Models
{
    public class MailDbContext : DbContext
    {

        public MailDbContext() : base("mailString")
        {
        }

        public DbSet<MailModel> Mails { get; set; }



    }
}