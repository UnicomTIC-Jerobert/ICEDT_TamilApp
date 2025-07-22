using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICEDT_TamilApp.Domain.Entities
{
    public class MainActivityType
    {
        [Key]
        public int MainActivityTypeId { get; set; }
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public int LessonId { get; set; }
        [ForeignKey("LessonId")]
        public virtual Lesson? Lesson { get; set; }

        public ICollection<Activity> ?Activities { get; set; }
    }
}
