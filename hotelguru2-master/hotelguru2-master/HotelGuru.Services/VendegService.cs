using HotelGuru.DataContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Generators;
using HotelGuru.DataContext.Dtos;

namespace HotelGuru.Services
{
    public interface IVendegService
    {
        Task<VendegGetDto> GetVendegAsync(int id);
        Task<VendegGetDto> UpdateVendegAsync(int id, VendegModifyDto vendegDto);
    }

    public class VendegService : IVendegService
    {
        private readonly AppDbContext _context;

        public VendegService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<VendegGetDto> GetVendegAsync(int id)
        {
            var vendeg = await _context.Vendegek.FindAsync(id);
            return new VendegGetDto
            {
                Id = vendeg.Id,
                Felhasznalonev = vendeg.Felhasznalonev,
                TeljesNev = vendeg.TeljesNev,
                Telefonszam = vendeg.Telefonszam,
                Lakcim = vendeg.Lakcim,
                Email = vendeg.Email
            };
        }

        public async Task<VendegGetDto> UpdateVendegAsync(int id, VendegModifyDto vendegDto)
        {
            var vendeg = await _context.Vendegek.FindAsync(id);
            vendeg.Telefonszam = vendegDto.Telefonszam;
            vendeg.Lakcim = vendegDto.Lakcim;
            vendeg.Email = vendegDto.Email;

            await _context.SaveChangesAsync();

            return new VendegGetDto
            {
                Id = vendeg.Id,
                Felhasznalonev = vendeg.Felhasznalonev,
                TeljesNev = vendeg.TeljesNev,
                Telefonszam = vendeg.Telefonszam,
                Lakcim = vendeg.Lakcim,
                Email = vendeg.Email
            };
        }
    }
}
