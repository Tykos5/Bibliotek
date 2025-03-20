using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotek
{
    public class Bok
    {
        public string Förnamn { get; set; }
        public string Efternamn { get; set; }
        public string Titel { get; set; }
        public int Exemplar { get; set; } 

        // Konstruktor
        public Bok(string förnamn, string efternamn, string titel, int exemplar)
        {
            Förnamn = förnamn;
            Efternamn = efternamn;
            Titel = titel;
            Exemplar = exemplar;
        }

        // Skriv ut bokinformation
        public void listaBok()
        {
            if (Exemplar > 1) { Console.WriteLine($"{Förnamn} {Efternamn}, {Titel} - ({Exemplar}x)"); }
            else { Console.WriteLine($"{Förnamn} {Efternamn}, {Titel}"); }
        }

        // Skriv ut bokinformation för lånade böcker
        public void listaBokLånade()
        {
            if (Exemplar > 1) { Console.WriteLine($"{Förnamn} {Efternamn}, {Titel} - ({Exemplar}x) (utlånad)"); }
            else { Console.WriteLine($"{Förnamn} {Efternamn}, {Titel} (utlånad)"); }
        }
    }
}
