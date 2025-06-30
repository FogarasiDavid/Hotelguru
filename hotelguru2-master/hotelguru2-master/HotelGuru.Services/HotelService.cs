using HotelGuru.DataContext.Context;
using HotelGuru.DataContext.Entities;
using HotelGuru.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;

namespace HotelGuru.Services
{
    public interface IHotelServices
    {
        Task<List<SzobaGetDto>> GetAvailableRoomsAsync();
        Task<List<FoglalasGetDto>> GetAllBookingsAsync();
    }

    public class HotelServices : IHotelServices
    {
        private readonly AppDbContext _context;

        public HotelServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SzobaGetDto>> GetAvailableRoomsAsync()
        {
            return await _context.Szobak
                .Where(sz => sz.Foglalhato)
                .Select(sz => new SzobaGetDto
                {
                    Id = sz.Id,
                    Szobaszam = sz.Szobaszam,
                    Tipus = sz.Tipus,
                    FerohelyekSzama = sz.FerohelyekSzama,
                    Felszereltseg = sz.Felszereltseg,
                    Foglalhato = sz.Foglalhato
                })
                .ToListAsync();
        }

        public async Task<List<FoglalasGetDto>> GetAllBookingsAsync()
        {
            return await _context.Foglalasok
                .Include(f => f.Vendeg)
                .Include(f => f.Szoba)
                .Select(f => new FoglalasGetDto
                {
                    Id = f.Id,
                    VendegId = f.VendegId,
                    SzobaId = f.SzobaId,
                    ErkezesDatum = f.ErkezesDatum,
                    TavozasDatum = f.TavozasDatum,
                    Visszaigazolva = f.Visszaigazolva,
                    LemondasiIdo = f.LemondasiIdo
                })
                .ToListAsync();
        }
    }
}
