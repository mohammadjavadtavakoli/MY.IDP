using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MY.WebApi.ImageGallery.Models;
using MY.WebApo.ImageGallery.Service;
using MY.WebApi.ImageGallery.DomainClasses;

namespace MY.WebApi.ImageGallery.WebApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _hostEnvironment;

        public ImageController(IImageService imageService, IMapper mapper, IHostEnvironment hostEnvironment)
        {
            _imageService = imageService;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetImages()
        {
            var ownerId = this.User.Claims.FirstOrDefault(claim => claim.Type == "sub").Value;
            var images = await _imageService.GetImagesAsync(ownerId);
            
            var imageToReturn = _mapper.Map<IEnumerable<ImageModel>>(images);
            return Ok(imageToReturn);
        }

        [HttpGet("{id}", Name = "GetImage")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var image = await _imageService.GetImageAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            var imageReturn = _mapper.Map<ImageModel>(image);
            return Ok(imageReturn);
        }
        [HttpPost(Name = "image")]
        [Authorize(Roles = "PayingUser")]
        public async Task<IActionResult> CreateImage([FromBody] ImageForCreationModel imageForCreationModel)
        {
            if (imageForCreationModel == null)
            {
                return BadRequest();
            }

            var OwnerId = this.User.Claims.FirstOrDefault(x => x.Type == "sub").Value;

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var imageEntity = _mapper.Map<Image>(imageForCreationModel);

            var contentRootPath = _hostEnvironment.ContentRootPath;

            var fileName = $"{Guid.NewGuid().ToString()}.jpg";

            var fullFilePath = Path.Combine($"{contentRootPath}/image/{fileName}");

            System.IO.File.WriteAllBytes(fullFilePath, imageForCreationModel.Bytes);

            imageEntity.FileName = fileName;
            imageEntity.OwnerId = OwnerId;

            await _imageService.AddImageAsync(imageEntity);

            var imageToReturn = _mapper.Map<Image>(imageEntity);
            return CreatedAtRoute("GetImage", new { id = imageToReturn.Id }, imageToReturn);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteImage(Guid id)
        {
            var image = await _imageService.GetImageAsync(id);
            if (image == null)
            {
                return BadRequest();
            }

            await _imageService.DeleteImageAsync(image);
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImage(Guid id, ImageForUpdateModel imageForUpdate)
        {
            if (imageForUpdate == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);

            }
            var image = await _imageService.GetImageAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            _mapper.Map(imageForUpdate, image);
            await _imageService.UpdateImageAsync(image);
            return NoContent();
        }
    }
}