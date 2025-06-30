using HotelGuru.DataContext.Context;
using HotelGuru.DataContext.Dtos;
using HotelGuru.DataContext.Entities;
using HotelGuru.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelGuru.Models;

namespace HotelGuru.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // alapból védett
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ValidateUser _validator;
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Felhasznalo> _passwordHasher;

        public AuthController(
            IConfiguration configuration,
            ValidateUser validator,
            AppDbContext context,
            IPasswordHasher<Felhasznalo> passwordHasher)
        {
            _configuration = configuration;
            _validator = validator;
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisztracioDto dto)
        {
            // Ellenőrizzük, hogy nincs-e már ilyen felhasználónév
            if (await _context.Vendegek
                    .AnyAsync(u => u.Felhasznalonev == dto.FelhasznaloNev))
            {
                return BadRequest("Ez a felhasználónév már foglalt.");
            }

            // Új Vendeg entitás létrehozása
            var user = new Vendeg
            {
                Felhasznalonev = dto.FelhasznaloNev,
                TeljesNev = dto.TeljesNev
                // Itt lehetne még Email, Telefonszam stb.
            };

            // Jelszó hashelése
            user.JelszoHash = _passwordHasher.HashPassword(user, dto.Jelszo);

            _context.Vendegek.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Regisztráció sikeres." });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] BejelentkezesDto dto)
        {
            try
            {
                var user = await _validator.ValidateAsync(dto.FelhasznaloNev, dto.Jelszo);
                if (user == null)
                    return Unauthorized("Érvénytelen felhasználónév vagy jelszó.");

                var jwtSection = _configuration.GetSection("JwtSettings");
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSection["SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Discriminator alapján role
                var role = _context.Entry(user)
                                   .Property("Discriminator")
                                   .CurrentValue
                                   ?.ToString();

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, dto.FelhasznaloNev),
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, role ?? "Vendeg"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var tokenObj = new JwtSecurityToken(
                    issuer: jwtSection["Issuer"],
                    audience: jwtSection["Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSection["ExpiresInMinutes"])),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(tokenObj),
                    ervenyes = tokenObj.ValidTo
                });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }
    }
}
