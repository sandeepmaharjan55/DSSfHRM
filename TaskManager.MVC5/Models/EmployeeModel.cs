using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManager.MVC5.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string designation { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set;}
        public string ContactNo { get; set; }
        public DateTime enrollDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Status { get; set; }
        public Skill Skill { get; set; }
        public int? SkillId { get; set; }
        public int? Rank { get; set; }
    }

    public class EmployeeViewModel
    {
        public EmployeeViewModel()
        {
            skills = new List<SkillViewModel>();
        }
        public int Id { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Designation")]
        public string designation { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DisplayName("Phone")]
        public string ContactNo { get; set; }
        [Required]
        [DisplayName("Enroll Date")]
        public DateTime enrollDate { get; set; }
        public bool Status { get; set; }
        public int? SkillId { get; set; }

        public List<SkillViewModel> skills { get; set; }
    }

    public class attendance
    {
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public int EmpId { get; set; }
        public DateTime date { get; set; }
        public char remark { get; set; }
}
        
}