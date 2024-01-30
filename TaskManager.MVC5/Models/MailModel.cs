using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManager.MVC5.Models
{
    public class MailModel
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

}