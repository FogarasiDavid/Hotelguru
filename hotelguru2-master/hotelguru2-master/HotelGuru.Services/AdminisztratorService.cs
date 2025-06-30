using HotelGuru.DataContext.Context;
using HotelGuru.DataContext.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HotelGuru.Services
{
    public interface IAdminisztratorService
    {
        Task<List<AdminGetDto>> GetAllAsync();
        Task<AdminGetDto> GetByIdAsync(int id);
    }
    public class AdminisztratorService : IAdminisztratorService
    {
        private readonly AppDbContext _context;

        public AdminisztratorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdminGetDto>> GetAllAsync()
        {
            return await _context.Adminisztratorok.Select(a => new AdminGetDto
            {
                Id = a.Id,
                Felhasznalonev = a.Felhasznalonev,
                TeljesNev = a.TeljesNev
            }).ToListAsync();
        }

        public async Task<AdminGetDto> GetByIdAsync(int id)
        {
            var a = await _context.Adminisztratorok.FindAsync(id);
            return new AdminGetDto
            {
                Id = a.Id,
                Felhasznalonev = a.Felhasznalonev,
                TeljesNev = a.TeljesNev
            };
        }
    }

}
