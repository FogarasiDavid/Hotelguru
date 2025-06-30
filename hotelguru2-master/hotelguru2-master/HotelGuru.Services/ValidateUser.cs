using HotelGuru.DataContext.Context;
using HotelGuru.DataContext.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HotelGuru.Services
{
    public class ValidateUser
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Felhasznalo> _passwordHasher;

        public ValidateUser(AppDbContext context,
                            IPasswordHasher<Felhasznalo> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<Felhasznalo?> ValidateAsync(string username, string password)
        {
            // Egyetlen lekérdezés minden Discriminator-ral
            var user = await _context.Felhasznalok
                .SingleOrDefaultAsync(u => u.Felhasznalonev == username);

            if (user == null)
                return null;

            // Hash-elt jelszó ellenőrzése
            var result = _passwordHasher.VerifyHashedPassword(user, user.JelszoHash, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }
    }
}
