using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class TrainingCourse
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public string Duration { get; set; }
        public int TestDuration { get; set; }
        public string Logo { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
