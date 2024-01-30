using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TaskManager.MVC5.Model;

namespace TaskManager.MVC5.Models
{
    public class TaskDetails
    {
        public Task Task { get; set; }
        public IEnumerable<Status> Statuses { get; set; }
        public TaskDetails(Task ts, IEnumerable<Status> st)
        {
            Task = ts;
            Statuses = st;
        }


    }
}