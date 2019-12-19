using Microsoft.AspNetCore.Http;

namespace CMWeb.Models
{
    public class FileInputModel
    {
        public IFormFile FileToUpload { get; set; }
    }

}