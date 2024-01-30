namespace TaskManager.MVC5.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("Statuses")]
    public class Status
    {
        public int id { get; set; }

        [StringLength(50)]
        public string title { get; set; }

        [StringLength(10)]
        public string color { get; set; }
    }
}
