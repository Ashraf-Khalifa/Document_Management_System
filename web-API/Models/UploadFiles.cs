namespace web_API.Models
{
    public class UploadFiles
    {
        public Guid Id { get; set; }
        public string FileId { get; internal set; }
        public string Name { get; internal set; }
        public string FileType { get; internal set; }
        public DateTime CreationDate { get; internal set; }
        public string FileName { get; internal set; }
        public string ContentType { get; internal set; }
    }
}
