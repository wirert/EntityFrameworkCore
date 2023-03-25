namespace TeisterMask.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class EmployeeTask
    {
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public int TaskId { get; set; }

        [ForeignKey(nameof(TaskId))]
        public virtual Task Task { get; set; } = null!;
    }
}
