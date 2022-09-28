using AutoMapper;
using MY.WebApi.ImageGallery.DomainClasses;
using MY.WebApi.ImageGallery.Models;

namespace MY.WebApi.ImageGallery.Mappings
{
    public class ImageMappingsProfile:Profile
    {
        public ImageMappingsProfile()
        {
            this.CreateMap<Image, ImageModel>().ReverseMap();
        }
    }
}

