using Microsoft.EntityFrameworkCore;
using MY.WebApi.ImageGallery.DataLayer.Context;
using MY.WebApi.ImageGallery.DomainClasses;

namespace MY.WebApo.ImageGallery.Service
{
    public interface IImageService
    {
        Task<bool> ImageExistsAsync(Guid id);
        Task<Image> GetImageAsync(Guid id);
        Task<List<Image>> GetImagesAsync();
        Task<Image> AddImageAsync(Image image);
        Task UpdateImageAsync(Image image);
        Task DeleteImageAsync(Image image);
    }
    
    public class ImageService:IImageService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Image> _images;

        public ImageService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _images = _uow.Set<Image>();
        }

        public Task<bool> ImageExistsAsync(Guid id)
        {
            return _images.AnyAsync(i => i.Id == id);
        }

        public Task<Image> GetImageAsync(Guid id)
        {
            return _images.FirstOrDefaultAsync(i => i.Id == id);
        }

        public Task<List<Image>> GetImagesAsync()
        {
            return _images.OrderBy(i => i.Title).ToListAsync();
        }

        public async Task<Image> AddImageAsync(Image image)
        {
            var imageEntry = _images.Add(image);
            await _uow.SaveChangesAsync();
            return imageEntry.Entity;
        }

        public Task UpdateImageAsync(Image image)
        {
            return Task.CompletedTask;
        }

        public async Task DeleteImageAsync(Image image)
        {
            _images.Remove(image);
            await _uow.SaveChangesAsync();
        }
    }
}

