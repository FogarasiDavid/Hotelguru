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
    
    public interface IFoglalasService
{
    Task<List<FoglalasGetDto>> GetAllFoglalasokAsync();
    Task<FoglalasGetDto> GetFoglalasByIdAsync(int id);
    Task<FoglalasGetDto> CreateFoglalasAsync(FoglalasCreateDto dto);
    Task<bool> LemondasAsync(int id);
    Task<bool> VisszaigazolasAsync(int id);
    Task<bool> SzobaSzabadEAsync(int szobaId, DateTime erkezes, DateTime tavozas);


    }


    public class FoglalasService : IFoglalasService
    {
        private readonly AppDbContext _context;

        public FoglalasService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FoglalasGetDto>> GetAllFoglalasokAsync()
        {
            return await _context.Foglalasok.Select(f => new FoglalasGetDto
            {
                Id = f.Id,
                VendegId = f.VendegId,
                SzobaId = f.SzobaId,
                ErkezesDatum = f.ErkezesDatum,
                TavozasDatum = f.TavozasDatum,
                Visszaigazolva = f.Visszaigazolva,
                LemondasiIdo = f.LemondasiIdo
            }).ToListAsync();
        }

        public async Task<FoglalasGetDto> GetFoglalasByIdAsync(int id)
        {
            var f = await _context.Foglalasok.FindAsync(id);
            return new FoglalasGetDto
            {
                Id = f.Id,
                VendegId = f.VendegId,
                SzobaId = f.SzobaId,
                ErkezesDatum = f.ErkezesDatum,
                TavozasDatum = f.TavozasDatum,
                Visszaigazolva = f.Visszaigazolva,
                LemondasiIdo = f.LemondasiIdo
            };
        }

        public async Task<FoglalasGetDto> CreateFoglalasAsync(FoglalasCreateDto dto)
        {
            if (dto.ErkezesDatum >= dto.TavozasDatum)
                throw new InvalidOperationException("Az érkezési dátumnak korábbinak kell lennie a távozásinál.");

            var utkozik = await _context.Foglalasok.AnyAsync(f =>
                f.SzobaId == dto.SzobaId &&
                f.TavozasDatum > dto.ErkezesDatum &&
                f.ErkezesDatum < dto.TavozasDatum);

            if (utkozik)
                throw new InvalidOperationException("Ez a szoba már foglalt ebben az időszakban.");

            var foglalas = new Foglalas
            {
                VendegId = dto.VendegId,
                SzobaId = dto.SzobaId,
                ErkezesDatum = dto.ErkezesDatum,
                TavozasDatum = dto.TavozasDatum,
                Visszaigazolva = false,
                LemondasiIdo = dto.ErkezesDatum.AddDays(-2)
            };

            _context.Foglalasok.Add(foglalas);

            var szoba = await _context.Szobak.FindAsync(dto.SzobaId);
            if (szoba != null)
            {
                szoba.Foglalhato = false;
            }

            await _context.SaveChangesAsync();

            return await GetFoglalasByIdAsync(foglalas.Id);
        }

        public async Task<bool> SzobaSzabadEAsync(int szobaId, DateTime erkezes, DateTime tavozas)
        {
            return !await _context.Foglalasok.AnyAsync(f =>
                f.SzobaId == szobaId &&
                f.TavozasDatum > erkezes &&
                f.ErkezesDatum < tavozas);
        }


        public async Task<bool> LemondasAsync(int id)
        {
            var foglalas = await _context.Foglalasok.FindAsync(id);
            if (foglalas == null || foglalas.LemondasiIdo < DateTime.Now)
                return false;

            _context.Foglalasok.Remove(foglalas);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VisszaigazolasAsync(int id)
        {
            var foglalas = await _context.Foglalasok.FindAsync(id);
            if (foglalas == null) return false;

            foglalas.Visszaigazolva = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }


}
