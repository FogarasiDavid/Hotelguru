using HotelGuru.Services;
using HotelGuru.DataContext.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HotelGuru.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecepciosController : ControllerBase
{
    private readonly IRecepciosService _service;

    public RecepciosController(IRecepciosService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _service.GetByIdAsync(id));
}
