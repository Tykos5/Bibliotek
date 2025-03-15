using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotek
{
    internal class Program
    {
        static List<Bok> mittBibliotek = new List<Bok>();
        static void Main(string[] args)
        {
            bool avsluta = false;

           

            while (!avsluta)
            {
                //Skriv bok infon till fil
                mittBibliotek.Clear(); // Rensar först personlistan innan användning
                StreamReader läsfil = new StreamReader("Bibliotek.txt");
                string s;
                // Läser filen tills till slutet hittas
                while ((s = läsfil.ReadLine()) != null)
                {
                    string[] bokdata = s.Split(','); // Delar upp data i raden för array

                    mittBibliotek.Add(new Bok(bokdata[0], bokdata[1], bokdata[2])); // Läser in data till objektet
                }
                läsfil.Close();

                bool meny = true;
                while (meny)
                {
                    meny = false;
                    Console.WriteLine("Välkommen till ditt personliga bibliotek, vad vill du göra? \n\n 1. Lista alla böcker i ditt bibliotek. \n 2. Lägg till ny bok. \n 3. Ta bort bok \n 4. Sök efter existerande bok. \n 5. Låna en bok. \n 6. Återlämna en bok. \n 7. Avsluta program. \n\nSvara med en siffra 1-7");
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            ListaBok();
                            break;
                        /* case "2":
                             NyBok();
                             break;
                         case "3":
                             BortBok;
                             break;
                         case "4":
                             Sökbok();
                             break;
                         case "5":
                             LånaBok();
                             break;
                         case "6":
                             Återlämnabok
                        break;*/
                        case "7":
                            avsluta = true;
                            break;
                        default:
                            Console.WriteLine("Felaktig input försök igen");
                            meny = true;
                            break;
                    }
                } //MENY AVSLUTAS
            }
        }
        static void ListaBok()
        {
            //Skriv ut info om böckerna
            for (int i = 0; i < mittBibliotek.Count; i++)
            {
                mittBibliotek[i].listaBok();
            }
        }
    }
}
