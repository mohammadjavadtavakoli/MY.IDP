using AutoMapper;
using MY.WebApi.ImageGallery.DomainClasses;
using MY.WebApi.ImageGallery.Models;

namespace MY.WebApi.ImageGallery.Mappings
{
    public class ImageForCreationMappingsProfile: Profile
    {
        public ImageForCreationMappingsProfile()
        {
            this.CreateMap<ImageForCreationModel, Image>()
                .ForMember(m => m.FileName, options => options.Ignore())
                .ForMember(m => m.Id, options => options.Ignore())
                .ForMember(m => m.OwnerId, options => options.Ignore());
        }
    }
}

