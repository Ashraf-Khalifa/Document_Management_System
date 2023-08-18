using System;

namespace web_API.Models
{
    public class Files1
    {
        public Guid Id { get; set; }
        public string FileId { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
