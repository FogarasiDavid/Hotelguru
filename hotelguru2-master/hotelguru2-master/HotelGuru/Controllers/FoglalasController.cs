using HotelGuru.Services;
using HotelGuru.DataContext.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HotelGuru.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoglalasController : ControllerBase
{
    private readonly IFoglalasService _service;

    public FoglalasController(IFoglalasService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllFoglalasokAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _service.GetFoglalasByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(FoglalasCreateDto dto)
    {
        try
        {
            var result = await _service.CreateFoglalasAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("lemondas/{id}")]
    public async Task<IActionResult> Lemondas(int id)
        => Ok(await _service.LemondasAsync(id));

    [Authorize(Roles = "Admin,Recepcios")]
    [HttpPut("visszaigazolas/{id}")]
    public async Task<IActionResult> Visszaigazolas(int id)
        => Ok(await _service.VisszaigazolasAsync(id));


    [HttpGet("szobaszabad")]
    public async Task<IActionResult> SzobaSzabad([FromQuery] int szobaId, [FromQuery] DateTime erkezes, [FromQuery] DateTime tavozas)
    {
        var szabad = await _service.SzobaSzabadEAsync(szobaId, erkezes, tavozas);
        return Ok(szabad);
    }
}
