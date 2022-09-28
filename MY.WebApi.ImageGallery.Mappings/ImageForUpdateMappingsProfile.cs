using AutoMapper;
using MY.WebApi.ImageGallery.DomainClasses;
using MY.WebApi.ImageGallery.Models;

namespace MY.WebApi.ImageGallery.Mappings
{
    public class ImageForUpdateMappingsProfile:Profile
    {
        public ImageForUpdateMappingsProfile()
        {
            this.CreateMap<ImageForUpdateModel, Image>()
                .ForMember(m => m.FileName, options => options.Ignore())
                .ForMember(m => m.Id, options => options.Ignore())
                .ForMember(m => m.OwnerId, options => options.Ignore());
        }
    }
}

