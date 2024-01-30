using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManager.MVC5.Models
{
    public class Report
    {
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public Taskss Taskss { get; set; }
        public int? EmployeeId { get; set; }
        public int? TaskId { get; set; }
        public int? Progress { get; set; }
        public string Description { get; set; }
        public DateTime ReportDate { get; set; }

    }

    public class ReportViewModel
    {
        public int Id { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int TaskId { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public DateTime ReportDate { get; set; }

    }
}