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
                         case "2":
                             NyBok();
                             break;
                         case "3":
                             BortBok();
                             break;
                         case "4":
                             SökBok();
                             break;
                        /*case "5":
                            LånaBok();
                            break;
                        case "6":
                            ÅterlämnaBok();
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
            Console.WriteLine("\n");
        }

        static void NyBok()
        {
            Console.WriteLine("Vad är författarens förnamn?");
            string förNamn = Console.ReadLine();
            Console.WriteLine("Vad är författarens efternamn?");
            string efterNamn = Console.ReadLine();
            Console.WriteLine("Vad är bokens titel?");
            string titel = Console.ReadLine();
            mittBibliotek.Add(new Bok(förNamn, efterNamn, titel));

            StreamWriter skrivfil = new StreamWriter("Bibliotek.txt");
            for (int i = 0; i < mittBibliotek.Count; i++)
            {
                skrivfil.WriteLine($"{mittBibliotek[i].Förnamn},{mittBibliotek[i].Efternamn},{mittBibliotek[i].Titel}");
            }
            skrivfil.Close();
        }

        static void BortBok()
        {
            Console.WriteLine("Vad heter boken du vill ta bort?");
            string input = Console.ReadLine();

            bool bokHittad = false;
            for (int i = 0; i < mittBibliotek.Count; i++)
            {
                // Kollar om titeln på boken matchar det användaren skriver
                if (mittBibliotek[i].Titel.Equals(input, StringComparison.OrdinalIgnoreCase))
                {
                    mittBibliotek.RemoveAt(i);
                    Console.WriteLine($"{input} togs bort från biblioteket");
                    bokHittad = true;
                    break; // När boken tas bort, avsluta loopen
                }
            }

            if (!bokHittad)
            {
                Console.WriteLine("Boken hittades inte i biblioteket.");
            }

            // Skriv tillbaka den uppdaterade listan till filen
            StreamWriter skrivfil = new StreamWriter("Bibliotek.txt");
            for (int i = 0; i < mittBibliotek.Count; i++)
            {
                skrivfil.WriteLine($"{mittBibliotek[i].Förnamn},{mittBibliotek[i].Efternamn},{mittBibliotek[i].Titel}");
            }
            skrivfil.Close();
        }

        static void SökBok()
        {
            bool sök = true;
            while (sök)
            {
                Console.WriteLine("Vill du söka efter förnamn eller efternamn?");
                string input = Console.ReadLine();
                bool författareHittad;

                switch (input.ToLower())
                {
                    case "efternamn":
                        författareHittad = false;
                        while (!författareHittad)
                        {
                            Console.WriteLine("Vilken författare vill du söka efter? (efternamn)");
                            string sökEfternamn = Console.ReadLine();

                            int i;
                            for (i = 0; i < mittBibliotek.Count; i++)
                            {
                                // Kollar om titeln på boken matchar det användaren skriver
                                if (mittBibliotek[i].Efternamn.Equals(sökEfternamn, StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.WriteLine($"Du har dessa böcker av {mittBibliotek[i].Efternamn} i ditt bibliotek:");
                                    Console.WriteLine($"{mittBibliotek[i].Förnamn} {mittBibliotek[i].Efternamn}, {mittBibliotek[i].Titel}");
                                    författareHittad = true;
                                    sök = false;
                                    break;
                                }
                            } 
                            for (i = i; i < mittBibliotek.Count; i++)
                            {
                                if (mittBibliotek[i].Efternamn.Equals(sökEfternamn, StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.WriteLine($"{mittBibliotek[i].Förnamn} {mittBibliotek[i].Efternamn}, {mittBibliotek[i].Titel}");
                                }
                            }

                            if (!författareHittad)
                            {
                                Console.WriteLine("Författaren hittades inte i biblioteket.");
                            }
                        }
                        break;
                    case "förnamn":

                        författareHittad = false;
                        while (!författareHittad)
                        {
                            Console.WriteLine("Vilken titel söker du efter?");
                            string sökTitel = Console.ReadLine();

                            int i;
                            for (i = 0; i < mittBibliotek.Count; i++)
                            {
                                // Kollar om titeln på boken matchar det användaren skriver
                                if (mittBibliotek[i].Titel.Equals(sökTitel, StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.WriteLine($"Du har dessa böcker av {mittBibliotek[i].Efternamn} i ditt bibliotek:");
                                    Console.WriteLine($"{mittBibliotek[i].Förnamn} {mittBibliotek[i].Efternamn}, {mittBibliotek[i].Titel}");
                                    författareHittad = true;
                                    sök = false;
                                    break;
                                }
                            }
                            for (i = i; i < mittBibliotek.Count; i++)
                            {
                                if (mittBibliotek[i].Efternamn.Equals(sökTitel, StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.WriteLine($"{mittBibliotek[i].Förnamn} {mittBibliotek[i].Efternamn}, {mittBibliotek[i].Titel}");
                                }
                            }

                            if (!författareHittad)
                            {
                                Console.WriteLine("Författaren hittades inte i biblioteket.");
                            }
                        }

                        break;
                    default:
                        Console.WriteLine("Felaktig input, Försök igen");
                        break;
                }
            }
            Console.WriteLine("\n");
        }
    }
}
