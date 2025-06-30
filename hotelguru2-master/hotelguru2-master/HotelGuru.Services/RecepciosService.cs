using HotelGuru.DataContext.Context;
using HotelGuru.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGuru.Services
{
    public interface IRecepciosService
    {
        Task<List<RecepciosGetDto>> GetAllAsync();
        Task<RecepciosGetDto> GetByIdAsync(int id);
    }
    public class RecepciosService : IRecepciosService
    {
        private readonly AppDbContext _context;

        public RecepciosService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RecepciosGetDto>> GetAllAsync()
        {
            return await _context.Recepciosok.Select(r => new RecepciosGetDto
            {
                Id = r.Id,
                Felhasznalonev = r.Felhasznalonev,
                TeljesNev = r.TeljesNev
            }).ToListAsync();
        }

        public async Task<RecepciosGetDto> GetByIdAsync(int id)
        {
            var r = await _context.Recepciosok.FindAsync(id);
            return new RecepciosGetDto
            {
                Id = r.Id,
                Felhasznalonev = r.Felhasznalonev,
                TeljesNev = r.TeljesNev
            };
        }
    }

}
