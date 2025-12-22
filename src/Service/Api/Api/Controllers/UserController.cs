using Api.Validation;
using Application.Services.DTO;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IImageService _imageService;

    public UserController(
        IUserService userService,
        IImageService imageService)
    {
        _userService = userService;
        _imageService = imageService;
    }

    /// <summary>
    /// Получить пользователя
    /// </summary>
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid userId)
    {
        var response = await _userService.GetUserAsync(userId);
        return Ok(response);
    }

    /// <summary>
    /// Создать пользователя
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var response = await _userService.CreateUserAsync(request);
        return StatusCode(201, response);
    }

    /// <summary>
    /// Обновить пользователя
    /// </summary>
    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] UpdateUserRequest request)
    {
        var response = await _userService.UpdateUserAsync(userId, request);
        return Ok(response);
    }

    /// <summary>
    /// Удалить пользователя
    /// </summary>
    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
    {
        await _userService.DeleteUserAsync(userId);
        return NoContent();
    }

    /// <summary>
    /// Присвоить рейтинг объявлению
    /// </summary>
    [HttpPost("{userId}/{advertisementId}/rating")]
    public async Task<IActionResult> SetRating(
        [FromRoute] Guid userId,
        [FromRoute] Guid advertisementId,
        [FromBody] RatingRequest request)
    {
        await _userService.SetRatingAsync(userId, advertisementId, request);
        return Ok();
    }

    /// <summary>
    /// Получить изображение
    /// </summary>
    [HttpGet("{userId}/{advertisementId}/image")]
    public async Task GetImage(
        [FromRoute] Guid userId,
        [FromRoute] Guid advertisementId,
        [FromQuery] string size = "original",
        CancellationToken cancellationToken = default)
    {
        var imageFileName = await _userService.GetImageNameAsync(userId, advertisementId);

        if (string.IsNullOrEmpty(imageFileName))
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            await Response.WriteAsync("Аватарка не найдена.", cancellationToken);
            return;
        }

        var contentType = Path.GetExtension(imageFileName).ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream"
        };
        
        Response.ContentType = contentType;
        Response.Headers.ContentDisposition = $"inline; filename=\"{imageFileName}\"";
        
        await _imageService.GetResizedImageAsync(size, imageFileName, Response.Body, cancellationToken);
    }

    /// <summary>
    /// Добавление объявления пользователем
    /// </summary>
    [HttpPost("{userId}/advertisements")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddAdvertisement(
        [FromRoute] Guid userId,
        [FromForm] CreateAdvertisementRequest request,
        [FromForm] IFormFile imageFile,
        [FromQuery] int width = 300)
    {
        string? uploadedFile = null;

        var validator = new ImageValidator();
        var validationResult = await validator.ValidateAsync(imageFile);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        
        try
        {
            using var stream = imageFile.OpenReadStream();
            uploadedFile = await _imageService.UploadImageAsync(stream, imageFile.FileName, imageFile.ContentType, width);
            request.Image = uploadedFile;

            await _userService.AddAdvertisementAsync(userId, request);
            return NoContent();
        }
        finally
        {
            if (uploadedFile != null && request.Image != uploadedFile)
            {
                await _imageService.DeleteImageAsync(uploadedFile);
            }
        }
    }

    /// <summary>
    /// Обновление объявления
    /// </summary>
    [HttpPut("{userId:guid}/advertisements/{advertisementId:guid}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateAdvertisement(
        [FromRoute] Guid userId,
        [FromRoute] Guid advertisementId,
        [FromForm] UpdateAdvertisementRequest request,
        [FromForm] IFormFile? imageFile,
        [FromQuery] int width = 300)
    {
        string? uploadedFile = null;

        try
        {
            if (imageFile != null)
            {
                var validator = new ImageValidator();
                var validationResult = await validator.ValidateAsync(imageFile);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

                using var stream = imageFile.OpenReadStream();
                var uploadedImage =
                    await _imageService.UploadImageAsync(stream, imageFile.FileName, imageFile.ContentType, width);
                request.Image = uploadedImage;
            }

            await _userService.UpdateAdvertisementAsync(userId, advertisementId, request);
            return NoContent();
        }
        finally
        {
            if (uploadedFile != null && request.Image != uploadedFile)
            {
                await _imageService.DeleteImageAsync(uploadedFile);
            }
        }
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