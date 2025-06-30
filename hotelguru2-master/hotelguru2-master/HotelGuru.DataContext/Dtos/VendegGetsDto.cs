using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGuru.DataContext.Dtos
{
    public class VendegGetDto
    {
        public int Id { get; set; }
        public string Felhasznalonev { get; set; }
        public string TeljesNev { get; set; }
        public string Telefonszam { get; set; }
        public string Lakcim { get; set; }
        public string Email { get; set; }
    }
}
