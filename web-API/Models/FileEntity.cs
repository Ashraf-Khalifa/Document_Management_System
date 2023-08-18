namespace web_API.Models
{
    public class FileEntity
    {
      
        public string FileName { get; set; }
        public string FileType { get; set; }
        public DateTime UploadDate { get; set; }
        public byte[] DataFiles { get; set; } // Store the file data as byte array
    }
}
