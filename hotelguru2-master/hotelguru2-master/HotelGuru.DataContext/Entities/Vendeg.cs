using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGuru.DataContext.Entities
{
    public class Vendeg : Felhasznalo
    {
        public string Telefonszam { get; set; }
        public string Lakcim { get; set; }
        public string Email { get; set; }
        public List<Foglalas> Foglalasok { get; set; }
    }
}
