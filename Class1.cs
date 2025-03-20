using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotek
{
    class Bok
    {
        public string Förnamn { get; set; }
        public string Efternamn { get; set; }
        public string Titel { get; set; }

        public Bok(string förnamn, string efternamn, string titel)
        {
            Titel = titel;
            Förnamn = förnamn;
            Efternamn = efternamn;
        }

        public void listaBok()
        {
            Console.WriteLine($"{Förnamn} {Efternamn}, {Titel}");
        }
        public void listaBokLånade()
        {
            Console.WriteLine($"{Förnamn} {Efternamn}, {Titel}  (Utlånad)");
        }
    }
}
