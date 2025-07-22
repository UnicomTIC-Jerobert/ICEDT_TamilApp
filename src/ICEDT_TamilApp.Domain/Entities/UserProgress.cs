using System.ComponentModel.DataAnnotations.Schema;
using ICEDT_TamilApp.Domain.Entities;

namespace ICEDT_TamilApp.Domain.Entities
{
    public class UserProgress
    {
        public long ProgressId { get; set; }


        public bool IsCompleted { get; set; }
        public int? Score { get; set; }
        public DateTime CompletedAt { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }


        public int ActivityId { get; set; }
        [ForeignKey("ActivityId")]
        public virtual Activity? Activity { get; set; }
    }
}