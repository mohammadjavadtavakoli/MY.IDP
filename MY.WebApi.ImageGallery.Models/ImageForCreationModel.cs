using System.ComponentModel.DataAnnotations;

namespace MY.WebApi.ImageGallery.Models
{
    public class ImageForCreationModel
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        public byte[] Bytes { get; set; }
    }
}

