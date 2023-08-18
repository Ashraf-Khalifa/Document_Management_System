using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_API.Data;
using web_API.Models;

namespace web_API.Controllers
{
    [ApiController] // This indicates that this class is an API controller.
    [Route("api/[controller]")] // Specifies the base route for this controller.
    public class ContactsController : ControllerBase
    {
        private readonly ContactsAPIDbContext dbContext; // Database context to interact with data.
        private long MaxFileSizeInBytes; // Maximum allowed file size in bytes.

        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
            MaxFileSizeInBytes = 5000 * 1024; // Set maximum file size limit to 5000 KB.
        }

        // This action retrieves URLs of files in a specific folder.
        [HttpGet("GetFiles")]
        public IActionResult GetFiles(string folderName)
        {
            try
            {
                List<string> FileUrls = new List<string>();
                string hostUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload");

                if (System.IO.Directory.Exists(folderPath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string filename = fileInfo.Name;
                        string filePath = Path.Combine(folderPath, filename);
                        if (System.IO.File.Exists(filePath))
                        {
                            string fileUrl = hostUrl + "/api/Contacts/DownloadPdfFile?filename=" + filename;
                            FileUrls.Add(fileUrl);
                        }
                    }
                }

                return Ok(FileUrls); // Returns the list of file URLs.
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes.
                return BadRequest("An error occurred while fetching the files.");
            }
        }

        

        // This action handles uploading multiple files.
        [HttpPost]
        [Route("UploadFiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Files1>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFiles(IEnumerable<IFormFile> files, string folderName, CancellationToken cancellationToken)
        {
            try
            {
                var uploadedFiles = new List<UploadFiles>();

                foreach (var file in files)
                {
                    if (file == null || file.Length <= 0)
                    {
                        return BadRequest("One or more files are empty.");
                    }

                    if (file.Length > MaxFileSizeInBytes)
                    {
                        return BadRequest($"File size exceeds the limit of {MaxFileSizeInBytes / 1024} KB.");
                    }

                    var allowedFileTypes = new[] { "image/jpeg", "image/png", "application/pdf" };
                    if (!allowedFileTypes.Contains(file.ContentType))
                    {
                        return BadRequest("Invalid file type.");
                    }

                    var fileId = await WriteFile(file, folderName);

                    var fileInfo = new UploadFiles
                    {
                        Id = Guid.NewGuid(),
                        FileId = fileId,
                        Name = file.FileName,
                        FileType = file.ContentType,
                        CreationDate = DateTime.UtcNow
                    };

                    uploadedFiles.Add(fileInfo);
                }

                // Save uploadedFiles to the database.
                foreach (var fileInfo in uploadedFiles)
                {
                    var fileEntity = new Files1
                    {
                        Id = fileInfo.Id,
                        FileId = fileInfo.FileId,
                        Name = fileInfo.Name,
                        FileType = fileInfo.FileType,
                        CreationDate = fileInfo.CreationDate
                    };

                    dbContext.Files1.Add(fileEntity);
                }

                await dbContext.SaveChangesAsync();

                return Ok(uploadedFiles); // Returns information about the uploaded files.
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes.
                return BadRequest("An error occurred while processing the files.");
            }
        }

        // This action downloads a PDF file from a specific folder.
        [HttpGet("DownloadPdfFile")]
        public async Task<IActionResult> DownloadPdfFile(string folderName, string filename)
        {
            try
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
                string filePath = Path.Combine(folderPath, filename);

                if (System.IO.File.Exists(filePath))
                {
                    var pdfBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    return File(pdfBytes, "application/pdf", filename); // Returns the PDF file.
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes.
                return NotFound();
            }
        }

        // This action deletes a file from a specific folder.
        [HttpDelete("remove")]
        public IActionResult RemoveFile(string folderName, string filename)
        {
            try
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
                string filePath = Path.Combine(folderPath, filename);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Ok("File removed successfully.");
                }
                else
                {
                    return NotFound("File not found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes.
                return BadRequest("An error occurred while removing the file.");
            }
        }

        // This private method writes an uploaded file to a specified folder.
        private async Task<string> WriteFile(IFormFile file, string folderPath)
        {
            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var exactpath = Path.Combine(folderPath, filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes.
            }
            return filename;
        }
    }
}
