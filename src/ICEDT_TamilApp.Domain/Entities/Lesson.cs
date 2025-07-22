using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICEDT_TamilApp.Domain.Entities
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }

        [Required]
        public string? LessonName { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int SequenceOrder { get; set; }


        [Required]
        public int LevelId { get; set; }
        [ForeignKey("LevelId")]
        public virtual Level? Level { get; set; }
        

    }
}