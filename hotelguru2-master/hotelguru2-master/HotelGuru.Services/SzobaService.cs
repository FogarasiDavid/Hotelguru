using HotelGuru.DataContext.Context;
using HotelGuru.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelGuru.DataContext.Dtos;

namespace HotelGuru.Services
{
    public interface ISzobaService
    {
        Task<List<SzobaGetDto>> GetAllSzobakAsync();
        Task<SzobaGetDto> GetSzobaByIdAsync(int id);
        Task<SzobaGetDto> CreateSzobaAsync(SzobaModifyDto dto);
        Task<SzobaGetDto> UpdateSzobaAsync(int id, SzobaModifyDto dto);
        Task<bool> DeleteSzobaAsync(int id);

    }
    public class SzobaService : ISzobaService
    {
        private readonly AppDbContext _context;

        public SzobaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SzobaGetDto>> GetAllSzobakAsync()
        {
            return await _context.Szobak.Select(s => new SzobaGetDto
            {
                Id = s.Id,
                Szobaszam = s.Szobaszam,
                Tipus = s.Tipus,
                FerohelyekSzama = s.FerohelyekSzama,
                Felszereltseg = s.Felszereltseg,
                Foglalhato = s.Foglalhato
            }).ToListAsync();
        }

        public async Task<SzobaGetDto> GetSzobaByIdAsync(int id)
        {
            var szoba = await _context.Szobak.FindAsync(id);
            return new SzobaGetDto
            {
                Id = szoba.Id,
                Szobaszam = szoba.Szobaszam,
                Tipus = szoba.Tipus,
                FerohelyekSzama = szoba.FerohelyekSzama,
                Felszereltseg = szoba.Felszereltseg,
                Foglalhato = szoba.Foglalhato
            };
        }
      

        public async Task<SzobaGetDto> CreateSzobaAsync(SzobaModifyDto dto)
        {
            var szoba = new Szoba
            {
                Szobaszam = dto.Szobaszam,
                Tipus = dto.Tipus,
                FerohelyekSzama = dto.FerohelyekSzama,
                Felszereltseg = dto.Felszereltseg,
                Foglalhato = dto.Foglalhato
            };
            _context.Szobak.Add(szoba);
            await _context.SaveChangesAsync();
            return await GetSzobaByIdAsync(szoba.Id);
        }

        public async Task<SzobaGetDto> UpdateSzobaAsync(int id, SzobaModifyDto dto)
        {
            var szoba = await _context.Szobak.FindAsync(id);
            szoba.Szobaszam = dto.Szobaszam;
            szoba.Tipus = dto.Tipus;
            szoba.FerohelyekSzama = dto.FerohelyekSzama;
            szoba.Felszereltseg = dto.Felszereltseg;
            szoba.Foglalhato = dto.Foglalhato;
            await _context.SaveChangesAsync();
            return await GetSzobaByIdAsync(id);
        }

        public async Task<bool> DeleteSzobaAsync(int id)
        {
            var szoba = await _context.Szobak.FindAsync(id);
            _context.Szobak.Remove(szoba);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
