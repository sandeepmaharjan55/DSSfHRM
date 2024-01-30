namespace TaskManager.MVC5.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Task
    {
        public int id { get; set; }

        [StringLength(256)]
        public string text { get; set; }

        public DateTime? start_date { get; set; }

        public DateTime? end_date { get; set; }

        [Column(TypeName = "text")]
        public string details { get; set; }

        public Guid? owner_id { get; set; }

        public Guid? creator_id { get; set; }

        public int? status_id { get; set; }
    }
}
