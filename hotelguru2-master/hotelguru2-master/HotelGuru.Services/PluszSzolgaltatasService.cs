using HotelGuru.DataContext.Context;
using HotelGuru.DataContext.Dtos;
using HotelGuru.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGuru.Services
{
    public interface IPluszSzolgaltatasService
    {
        Task<List<PluszSzolgaltatasGetDto>> GetAllAsync();
        Task<PluszSzolgaltatasGetDto> CreateAsync(PluszSzolgaltatasModifyDto dto);
        Task<bool> DeleteAsync(int id);
    }
    public class PluszSzolgaltatasService : IPluszSzolgaltatasService
    {
        private readonly AppDbContext _context;

        public PluszSzolgaltatasService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PluszSzolgaltatasGetDto>> GetAllAsync()
        {
            return await _context.PluszSzolgaltatasok.Select(p => new PluszSzolgaltatasGetDto
            {
                Id = p.Id,
                Nev = p.Nev,
                Leiras = p.Leiras,
                Ar = p.Ar
            }).ToListAsync();
        }

        public async Task<PluszSzolgaltatasGetDto> CreateAsync(PluszSzolgaltatasModifyDto dto)
        {
            var szolg = new PluszSzolgaltatas
            {
                Nev = dto.Nev,
                Leiras = dto.Leiras,
                Ar = dto.Ar
            };
            _context.PluszSzolgaltatasok.Add(szolg);
            await _context.SaveChangesAsync();

            return new PluszSzolgaltatasGetDto
            {
                Id = szolg.Id,
                Nev = szolg.Nev,
                Leiras = szolg.Leiras,
                Ar = szolg.Ar
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var szolg = await _context.PluszSzolgaltatasok.FindAsync(id);
            if (szolg == null) return false;

            _context.PluszSzolgaltatasok.Remove(szolg);
            await _context.SaveChangesAsync();
            return true;
        }
    }


}
