using System.ComponentModel.DataAnnotations;

namespace MY.ImageGallery.MvcClient.ViewModels
{
    public class EditImageViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public Guid Id { get; set; }
    }
}

