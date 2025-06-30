using HotelGuru.Services;
using HotelGuru.DataContext.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HotelGuru.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendegController : ControllerBase
{
    private readonly IVendegService _service;

    public VendegController(IVendegService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _service.GetVendegAsync(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, VendegModifyDto dto)
        => Ok(await _service.UpdateVendegAsync(id, dto));
}
