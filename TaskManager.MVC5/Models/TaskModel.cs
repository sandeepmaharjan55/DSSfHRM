using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManager.MVC5.Models
{
    public class Taskss
    {
        public int Id { get; set; }
        public string TaskTitle { get; set; }
        public string Description { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public Employee Employee { get; set; }
        public Skill skill { get; set; }
        public int? EmployeeId { get; set; }
        public int? SkillId { get; set; }
    }

    public class TaskViewModel
    {
        public TaskViewModel()
        {
            employees = new List<EmployeeViewModel>();
        }
        public int Id { get; set; }
        [Required]
        [DisplayName("Title")]
        public string TaskTitle { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DisplayName("Start Date")]
        public DateTime AddedDate { get; set; }
        [Required]
        [DisplayName("End Date")]
        public DateTime CompletionDate { get; set; }
        public int EmployeeId { get; set; }

        public List<EmployeeViewModel> employees { get; set; }
        
    }

    public class Skill
    {
        public int Id { get; set; }
        public string SkillName { get; set; }
        public bool status { get; set; }
        
    }

    public class SkillViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Skill")]
        public string SkillName { get; set; }
        public bool status { get; set; }
      

    }
}