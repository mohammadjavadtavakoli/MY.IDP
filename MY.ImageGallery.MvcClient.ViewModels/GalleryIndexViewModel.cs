using MY.WebApi.ImageGallery.Models;

namespace MY.ImageGallery.MvcClient.ViewModels
{
    public class GalleryIndexViewModel
    {
        public IEnumerable<ImageModel> Images { get; }

        public GalleryIndexViewModel(IEnumerable<ImageModel> images)
        {
            Images = images;
        }
    }
}

