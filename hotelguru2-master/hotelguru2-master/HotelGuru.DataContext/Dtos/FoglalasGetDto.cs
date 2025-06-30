using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGuru.DataContext.Dtos
{ 
     public class FoglalasGetDto
    {
        public int Id { get; set; }
        public int VendegId { get; set; }
        public int SzobaId { get; set; }
        public DateTime ErkezesDatum { get; set; }
        public DateTime TavozasDatum { get; set; }
        public bool Visszaigazolva { get; set; }
        public DateTime LemondasiIdo { get; set; }
    }

}
