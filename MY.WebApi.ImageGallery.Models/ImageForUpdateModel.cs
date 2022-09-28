using System.ComponentModel.DataAnnotations;

namespace MY.WebApi.ImageGallery.Models
{
    public class ImageForUpdateModel
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }
    }
}

