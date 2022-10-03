using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MY.ImageGallery.MvcClient.ViewModels
{
    public class AddImageViewModel
    {
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();

        [Required]
        public string Title { get; set; }

    }
}

