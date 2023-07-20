using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingSystem.Entities
{
    public class EmployeeProject
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int TimeInMinutes { get; set; }
        public DateTime WorkDate { get; set; }
        public string TaskDescription { get; set; }
    }
}
