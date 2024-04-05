using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelingTrips.Data;
using TravelingTrips.Models;

namespace TravelingTrips.Controllers;

[Route("api/pins")]
[ApiController]
public class MapPinsController : ControllerBase
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly DataContext _context;
    public MapPinsController(IHttpContextAccessor httpContextAccessor, IHttpContextAccessor contextAccessor, DataContext context)
    {
        _contextAccessor = contextAccessor;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetMapPins()
    {
        var principal = _contextAccessor.HttpContext.User;
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        var list = await _context.MapPins.Where(x => x.UserId == userId).ToListAsync();
        return Ok(list);
    }

    [HttpPost]
    public async Task<IActionResult> InsertMapPins(MapPin model) 
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Select(x => x.Value.Errors)
                   .Where(y => y.Count > 0)
                   .ToList();
            return BadRequest();
        }

        var principal = _contextAccessor.HttpContext.User;
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        model.UserId = userId;

        await _context.MapPins.AddAsync(model);
        await _context.SaveChangesAsync();

        return Ok(model);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteMapPin(double latitude, double longitude)
    {
        var principal = _contextAccessor.HttpContext.User;
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        // Find pin to delete
        var pinToDelete = await _context.MapPins.FirstOrDefaultAsync(x => x.UserId == userId && x.Latitude == latitude && x.Longitude == longitude);
        if (pinToDelete == null)
        {
            return NotFound();
        }

        // Delete pin from the database
        _context.MapPins.Remove(pinToDelete);
        await _context.SaveChangesAsync();

        return Ok();
    }
}