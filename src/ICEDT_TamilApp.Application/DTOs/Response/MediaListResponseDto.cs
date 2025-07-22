namespace ICEDT_TamilApp.Application.DTOs.Response
{
    public class MediaListResponseDto
    {
        public List<string> Files { get; set; }
        public int Count { get; set; }
        public string Folder { get; set; }
    }
}
