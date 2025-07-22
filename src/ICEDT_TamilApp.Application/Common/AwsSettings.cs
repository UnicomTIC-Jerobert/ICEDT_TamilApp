namespace ICEDT_TamilApp.Application.Common
{
    public class AwsSettings
    {
        public const string SectionName = "AWS";
        public string BucketName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
    }
}