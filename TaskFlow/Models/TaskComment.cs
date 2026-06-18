using System.ComponentModel.DataAnnotations.Schema;

namespace TaskFlow.Models
{
    public class TaskComment
    {
        public long Id { get; set; }

        public int EmployeeTaskId { get; set; }

        public string Comment { get; set; }

        public int CreatedByEmployeeId { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual EmployeeTask EmployeeTask { get; set; }
        public virtual Employee CreatedByEmployee { get; set; }
    }
}
