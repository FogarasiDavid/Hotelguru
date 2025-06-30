using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGuru.DataContext.Dtos
{
    public class SzobaGetDto
    {
        public int Id { get; set; }
        public string Szobaszam { get; set; }
        public string Tipus { get; set; }
        public int FerohelyekSzama { get; set; }
        public string Felszereltseg { get; set; }
        public bool Foglalhato { get; set; }
    }

}
