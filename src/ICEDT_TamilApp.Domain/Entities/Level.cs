using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICEDT_TamilApp.Domain.Entities
{
    public class Level
    {
        [Key]
        public int LevelId { get; set; }

        [Required]
        public required string LevelName { get; set; }

        [Required]
        public int SequenceOrder { get; set; }

        public ICollection<Lesson>? Lessons { get; set; }
    }
}
