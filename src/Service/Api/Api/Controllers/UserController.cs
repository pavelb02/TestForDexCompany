using Api.ValidationDTO;
using Application.Services.DTO;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Exceptions;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IImageService _imageService;

    public UserController(IUserService userService, IImageService imageService)
    {
        _userService = userService;
        _imageService = imageService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid userId)
    {
        var response = await _userService.GetUserAsync(userId, false);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var response = await _userService.CreateUserAsync(request);
        return StatusCode(201, response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var response = await _userService.UpdateUserAsync(request);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
    {
        await _userService.DeleteUserAsync(userId);
        return NoContent();
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] AdvertisementSearchRequest request)
    {
        var advertisements = await _userService.SearchAsync(request);

        return Ok(advertisements);
    }
    
    [HttpPost("{userId}/{advertisementId}/rating")]
    public async Task<IActionResult> SetRating([FromBody] RatingRequest request)
    {
        await _userService.SetRatingAsync(request);
        return Ok();
    }

    [HttpGet("{userId}/{advertisementId}/image")]
    public async Task<IActionResult> GetImage(
        [FromRoute] Guid userId,
        [FromRoute] Guid advertisementId,
        [FromQuery] string size = "original")
    {
        var imageFileName = await _userService.GetImageNameAsync(userId, advertisementId);

        if (string.IsNullOrEmpty(imageFileName))
            return NotFound("Аватарка не найдена.");

        var result = await _imageService.GetResizedImageAsync(size, imageFileName);

        var (stream, fileName) = result;

        var contentType = Path.GetExtension(fileName).ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream"
        };

        return File(stream, contentType, fileName);
    }

    /// <summary>
    /// Добавление объявления пользователем
    /// </summary>
    [HttpPost("{userId}/advertisements")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddAdvertisement(
        [FromRoute] Guid userId,
        [FromForm] CreateAdvertisementRequest request,
        IFormFile imageFile)
    {
        try
        {
            var validator = new ImageValidator();
            var validationResult = await validator.ValidateAsync(imageFile);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            using var stream = imageFile.OpenReadStream();
            var avatarUrl = await _imageService.UploadImageAsync(stream, imageFile.FileName, imageFile.ContentType);
            request.Image = avatarUrl;
            
            await _userService.AddAdvertisementAsync(userId, request);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Errors = ex.Message });
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { Errors = ex.Message });
        }
    }

    /// <summary>
    /// Обновление объявления
    /// </summary>
    [HttpPut("{userId}/advertisements/{advertisementId}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateAdvertisement(
        [FromRoute] Guid userId,
        [FromForm] UpdateAdvertisementRequest request,
        IFormFile? imageFile)
    {
        if (imageFile != null)
        {
            var validator = new ImageValidator();
            var validationResult = await validator.ValidateAsync(imageFile);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            using var stream = imageFile.OpenReadStream();
            var fileName = await _imageService.UploadImageAsync(stream, imageFile.FileName, imageFile.ContentType);
            request.Image = fileName;
        }

        await _userService.UpdateAdvertisementAsync(userId, request);
        return NoContent();
    }

    /// <summary>
    /// Удаление объявления
    /// </summary>
    [HttpDelete("{userId}/advertisements/{advertisementId}")]
    public async Task<IActionResult> DeleteAdvertisement([FromRoute] Guid userId, [FromRoute] Guid advertisementId)
    {
        await _userService.DeleteAdvertisementAsync(userId, advertisementId);
        return NoContent();
    }
}