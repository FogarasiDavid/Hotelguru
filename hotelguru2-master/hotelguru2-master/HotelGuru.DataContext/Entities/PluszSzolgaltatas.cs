using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGuru.DataContext.Entities
{
    public class PluszSzolgaltatas
    {
        public int Id { get; set; }
        public string Nev { get; set; }
        public string Leiras { get; set; }
        public decimal Ar { get; set; }
        public List<Foglalas> Foglalasok { get; set; }
    }
}
