namespace TaskManager.MVC5.Model
{
    using System;
    using System.Data.Entity;

    public class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext()
            : base("name=TaskManagerString")
        {
               }

        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                .ToTable("Task");

            modelBuilder.Entity<Status>()
                .ToTable("Status");
        }
    }

    public class TaskManagerDbContextInit : CreateDatabaseIfNotExists<TaskManagerDbContext>
    {
        protected override void Seed(TaskManagerDbContext context)
        {
            InitTables(context);
            base.Seed(context);
        }

        private void InitTables(TaskManagerDbContext context)
        {
            /*
            if you need to create data tables
             */
            context.Statuses.AddRange(new[]
            {
                new Status(){title = "Waiting",color = "#5B9BE0"}, 
                new Status(){title = "In progress",color = "#FE7510"},
                new Status(){title = "Done",color = "#76B007"}
            });

        }
    }
}
