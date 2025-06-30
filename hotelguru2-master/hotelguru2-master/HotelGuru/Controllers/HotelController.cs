using HotelGuru.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelGuru.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelController : ControllerBase
{
    private readonly IHotelServices _service;

    public HotelController(IHotelServices service)
    {
        _service = service;
    }

    [HttpGet("available-rooms")]
    public async Task<IActionResult> GetAvailableRooms()
        => Ok(await _service.GetAvailableRoomsAsync());

    [HttpGet("bookings")]
    public async Task<IActionResult> GetAllBookings()
        => Ok(await _service.GetAllBookingsAsync());
}
