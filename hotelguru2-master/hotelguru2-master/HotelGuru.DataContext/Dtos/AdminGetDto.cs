using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGuru.DataContext.Dtos
{
    public class AdminGetDto
    {
        public int Id { get; set; }
        public string Felhasznalonev { get; set; }
        public string TeljesNev { get; set; }
    }

}
