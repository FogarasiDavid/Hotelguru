using System.ComponentModel.DataAnnotations;

namespace HotelGuru.DataContext.Entities
{
    public abstract class Felhasznalo
    {
        public int Id { get; set; }

        [Required]
        public string Felhasznalonev { get; set; }

        [Required]
        public string JelszoHash { get; set; }

        public string TeljesNev { get; set; }
    }

    public class Recepcios : Felhasznalo
    {
        // ide tehetsz plusz mezőket, ha kell
    }
}
