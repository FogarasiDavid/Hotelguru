using HotelGuru.Services;
using HotelGuru.DataContext.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HotelGuru.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SzobaController : ControllerBase
{
    private readonly ISzobaService _service;

    public SzobaController(ISzobaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllSzobakAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _service.GetSzobaByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(SzobaModifyDto dto)
        => Ok(await _service.CreateSzobaAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SzobaModifyDto dto)
        => Ok(await _service.UpdateSzobaAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => Ok(await _service.DeleteSzobaAsync(id));

}
