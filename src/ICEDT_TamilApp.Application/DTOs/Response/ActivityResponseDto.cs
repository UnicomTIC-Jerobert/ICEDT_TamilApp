namespace ICEDT_TamilApp.Application.DTOs.Response
{
    public class ActivityResponseDto
    {
        public int ActivityId { get; set; }
        public int LessonId { get; set; }
        public int ActivityTypeId { get; set; }
        public int MainActivityTypeId { get; set; }
        public required string Title { get; set; }
        public int SequenceOrder { get; set; }
        public required string ContentJson { get; set; }
    }
}
