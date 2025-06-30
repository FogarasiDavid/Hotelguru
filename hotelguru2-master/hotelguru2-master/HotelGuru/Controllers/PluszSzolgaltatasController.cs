using HotelGuru.Services;
using HotelGuru.DataContext.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HotelGuru.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PluszSzolgaltatasController : ControllerBase
{
    private readonly IPluszSzolgaltatasService _service;

    public PluszSzolgaltatasController(IPluszSzolgaltatasService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create(PluszSzolgaltatasModifyDto dto)
        => Ok(await _service.CreateAsync(dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => Ok(await _service.DeleteAsync(id));
}
