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
        [StringLength(50)]
        public required string Slug { get; set; }

        [Required]
        public int SequenceOrder { get; set; }

        public string? CoverImageUrl { get; set; }

        public ICollection<Lesson>? Lessons { get; set; }
    }
}
