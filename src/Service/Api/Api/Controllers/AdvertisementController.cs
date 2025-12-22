using Application.Services.DTO;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/advertisements")]
public class AdvertisementController : ControllerBase
{
    private readonly IAdvertisementService _advertisementService;
    
    public AdvertisementController(
        IAdvertisementService advertisementService)
    {
        _advertisementService = advertisementService;
    }
    
    /// <summary>
    /// Получить объявления (с фильтрацией, сортировкой и пагинацией)
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] AdvertisementSearchRequest request)
    {
        var results = await _advertisementService.SearchAsync(request);

        return Ok(results);
    }
}