using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bibliotek
{
    internal class Program
    {
        static List<Bok> mittBibliotek = new List<Bok>();
        static List<Bok> lånadeBöcker = new List<Bok>();


        static void Main(string[] args)
        {
            LaddaListor();
            bool avsluta = false;

            while (!avsluta)
            {
                Console.WriteLine("Välkommen till ditt personliga bibliotek! Vad vill du göra?");
                Console.WriteLine("1. Lista alla böcker");
                Console.WriteLine("2. Lägg till ny bok");
                Console.WriteLine("3. Ta bort bok");
                Console.WriteLine("4. Sök efter bok");
                Console.WriteLine("5. Låna en bok");
                Console.WriteLine("6. Återlämna en bok");
                Console.WriteLine("7. Redigera bok i bibliotek");
                Console.WriteLine("8. Avsluta program");
                Console.WriteLine("9. Rensa konsol");
                Console.Write("Välj ett alternativ (1-8): ");

                string input = Console.ReadLine();
                Console.Clear();

                switch (input)
                {
                    case "1": ListaBok(); break;
                    case "2": NyBok(); break;
                    case "3": BortBok(); break;
                    case "4": SökBok(); break;
                    case "5": LånaBok(); break;
                    case "6": ÅterlämnaBok(); break;
                    case "7": RedigeraBok(); break;
                    case "8": avsluta = true; break;
                    case "9": Console.Clear(); break;
                    default:
                        Console.WriteLine("Felaktig input, tryck för att försök igen");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }  //Menyval
            }
        }

        //laddar in listorna från filerna eller skapar dem om de inte finns + skriver in 13 böcker i biblioteket
        static void LaddaListor()
        {
            mittBibliotek.Clear();
            if (!File.Exists("Bibliotek.txt"))
            {
                using (FileStream fs = File.Create("Bibliotek.txt")) {}

                using (StreamWriter skrivfil = new StreamWriter("Bibliotek.txt"))  // Hårdkoda in 13 böcker i biblioteket då den skapas
                {
                    skrivfil.WriteLine("J.K.,Rowling,Harry Potter och de vises sten,1");
                    skrivfil.WriteLine("J.K.,Rowling,Harry Potter och hemligheternas kammare,1");
                    skrivfil.WriteLine("J.K.,Rowling,Harry Potter och fången från Azkaban,1");
                    skrivfil.WriteLine("J.K.,Rowling,Harry Potter och den flammande bägaren,1");
                    skrivfil.WriteLine("Harper,Lee,To kill a mockingbird,1");
                    skrivfil.WriteLine("Per Anders,Fogelström,Mina drömmars stad,5");
                    skrivfil.WriteLine("Jan,Guillou,Ondskan,3");
                    skrivfil.WriteLine("Astrid,Lindgren,Pippi Långstrump,1");
                    skrivfil.WriteLine("Astrid,Lindgren,Bröderna Lejonhjärta,4");
                    skrivfil.WriteLine("Astrid,Lindgren,Emil i Lönneberga,1");
                    skrivfil.WriteLine("Astrid,Lindgren,Ronja Rövardotter,1");
                    skrivfil.WriteLine("Astrid,Lindgren,Mio min Mio,1");
                    skrivfil.WriteLine("Khaled,Hosseini,Flyga drake,2");
                }
            }
            using (StreamReader läsfil = new StreamReader("Bibliotek.txt"))
            {
                string s;
                while ((s = läsfil.ReadLine()) != null)
                {
                    string[] bokdata = s.Split(',');
                    mittBibliotek.Add(new Bok(bokdata[0], bokdata[1], bokdata[2], int.Parse(bokdata[3])));
                }
            }

            lånadeBöcker.Clear();
            if (!File.Exists("Lånade.txt"))
            {
                using (FileStream fs = File.Create("Lånade.txt")) {}  // Skapar filen om den inte finns
            }
            else
            {
                using (StreamReader läsfil_L = new StreamReader("Lånade.txt"))
                {
                    string b;
                    while ((b = läsfil_L.ReadLine()) != null)
                    {
                        string[] bokdata = b.Split(',');
                        lånadeBöcker.Add(new Bok(bokdata[0], bokdata[1], bokdata[2], int.Parse(bokdata[3])));
                    }
                }
            }
        }


        //Menyalternativ
        static void ListaBok()
        {
            //Skriv ut info om böckerna
            foreach (var bok in mittBibliotek)
            {
                bok.listaBok();
            }
            Console.WriteLine();
            foreach (var bok in lånadeBöcker)
            {
                bok.listaBokLånade();
            }
            if (lånadeBöcker.Count > 0)
            {
                Console.WriteLine("\n");
            }
        }

        static void NyBok()
        {
            Console.WriteLine("Lägg till en ny bok i biblioteket:");
            string förNamn;
            do
            {
                Console.Write("Vad är författarens förnamn? ");
                förNamn = Console.ReadLine();
            } while (string.IsNullOrEmpty(förNamn));

            string efterNamn;
            do
            {
                Console.Write("Vad är författarens efternamn? ");
                efterNamn = Console.ReadLine();
            } while (string.IsNullOrEmpty(efterNamn));

            string titel;
            do
            {
                Console.Write("Vad är bokens titel? ");
                titel = Console.ReadLine();
            } while (string.IsNullOrEmpty(titel));

            var bok = mittBibliotek.FirstOrDefault(b => b.Titel.Equals(titel, StringComparison.OrdinalIgnoreCase) && b.Förnamn.Equals(förNamn, StringComparison.OrdinalIgnoreCase) && b.Efternamn.Equals(efterNamn, StringComparison.OrdinalIgnoreCase));
            
            if (bok != null)
            {
                // Om boken redan finns, öka antalet exemplar
                bok.Exemplar++;
                Console.WriteLine($"Antalet exemplar av {titel} ökat till {bok.Exemplar}.");
            }
            else
            {
                // Annars, lägg till en ny bok
                using (StreamWriter skrivfil = new StreamWriter("Bibliotek.txt", true))
                {
                    skrivfil.WriteLine($"{förNamn},{efterNamn},{titel},1");
                }
                mittBibliotek.Add(new Bok(förNamn, efterNamn, titel, 1));  // Lägg till den nya boken i listan
            }
        }

        static void BortBok()
        {
            while (true)
            {
                Console.WriteLine("Böcker i bibliotek:");
                foreach (var Bok in mittBibliotek)
                {
                    Bok.listaBok();
                }

                Console.WriteLine("\nVill du 1. ta bort en specifik bok, eller 2. ta bort alla böcker från en författare? (1/2)");
                string val = Console.ReadLine();
                if (val == "1")
                {
                    TaBortSpecifikBok();
                    return;
                }
                else if (val == "2")
                {
                    TaBortFörfattare();
                    return;
                }
                else
                {
                    Console.WriteLine("Felaktig input, tryck för att försöka igen");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void SökBok()
        {
            bool hittad = true;
            while (hittad)
            {
                hittad = false;
                Console.WriteLine("Vilken författare vill du söka efter? (ange författarens förnamn, efternamn eller titel)");
                string input = Console.ReadLine().ToLower();

                foreach (Bok bok in mittBibliotek)
                {
                    if (bok.Förnamn.ToLower().Contains(input) || bok.Efternamn.ToLower().Contains(input) || bok.Titel.ToLower().Contains(input))
                    {
                        hittad = true;
                        Console.WriteLine($"Författare: {bok.Förnamn} {bok.Efternamn}, Titel: {bok.Titel}");
                    }
                }
                Console.WriteLine();
                foreach (Bok bok in lånadeBöcker)
                {
                    if (bok.Förnamn.ToLower().Contains(input) || bok.Efternamn.ToLower().Contains(input))
                    {
                        hittad = true;
                        if (bok.Exemplar > 1)
                        {
                            Console.WriteLine($"Författare: {bok.Förnamn} {bok.Efternamn}, Titel: {bok.Titel} - ({bok.Exemplar}x) (utlånad)");
                        }
                        else
                        {
                            Console.WriteLine($"Författare: {bok.Förnamn} {bok.Efternamn}, Titel: {bok.Titel} (utlånad)");
                        }
                    }
                }
                Console.WriteLine();
                if (!hittad)
                {
                    while (true)
                    {
                        Console.WriteLine("Ingen bok hittades, vill du försöka igen? (ja/nej)");
                        string val = Console.ReadLine();

                        if ("ja".StartsWith(val, StringComparison.OrdinalIgnoreCase))
                        {
                            hittad = true;
                            break;
                        }
                        else if ("nej".StartsWith(val, StringComparison.OrdinalIgnoreCase))
                        {
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Fel input, tryck för att försöka igen");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
                }
                else { return;  }
                    Console.WriteLine("\n");
            }
        }

        static void LånaBok()
        {
            bool repris = true;
            while (repris)
            {
                Console.WriteLine("Böcker i bibliotek:");
                foreach (var Bok in mittBibliotek)
                {
                    Bok.listaBok();
                }

                Console.WriteLine("\nVilken bok vill du låna? Skriv in titeln");
                string input = Console.ReadLine();
                repris = false;


                var bok = mittBibliotek.FirstOrDefault(b => b.Titel.StartsWith(input, StringComparison.OrdinalIgnoreCase));


                if (bok != null)
                {
                    while (true)
                    {
                        Console.WriteLine($"Vill du låna {bok.Titel}? (Ja/Nej)");
                        string lån = Console.ReadLine();
                        if ("ja".StartsWith(lån, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"{bok.Titel} lånades ut, tryck för att fortsätta");
                            if (bok.Exemplar > 1)
                            {
                                bok.Exemplar--;
                            }
                            else
                            {
                                mittBibliotek.Remove(bok); // Tar bort boken från biblioteket 
                            }
                            bool finns = false;
                            foreach (Bok jämförbok in lånadeBöcker)
                            {
                                if (jämförbok.Titel == bok.Titel && jämförbok.Förnamn == bok.Förnamn && jämförbok.Efternamn == bok.Efternamn)
                                {
                                    jämförbok.Exemplar++;
                                    finns = true;
                                }
                            }
                            if (!finns)
                            {
                                lånadeBöcker.Add(new Bok(bok.Förnamn, bok.Efternamn,bok.Titel,1)); // Lägger till boken i listan med lånade böcker
                            }


                            SparaBibliotek();
                            SparaLånadeBöcker();
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }
                        else if ("nej".StartsWith(lån, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Boken lånades inte ut, tryck för att fortsätta");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Felaktig input, tryck för att försöka igen");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
                }
                
                else
                {
                    foreach (Bok Bok in lånadeBöcker)
                    {
                        bool finns = false;
                        if (Bok.Titel.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                        {
                            finns = true;
                            Console.WriteLine($"{Bok.Titel} är redan utlånad, tryck för att fortsätta");
                            Console.ReadKey();
                            Console.Clear();

                        }
                        if (!finns)
                        {
                            Console.WriteLine("Boken finns inte i biblioteket");
                            Console.WriteLine("Vill du försöka igen? (ja/nej)");
                            string försökIgen = Console.ReadLine();
                            if ("ja".StartsWith(försökIgen, StringComparison.OrdinalIgnoreCase))
                            {
                                repris = true;
                                Console.Clear();
                            }
                            else
                            {
                                repris = false;
                                return;
                            }
                        }
                    }
                }
            }
        }

        static void ÅterlämnaBok()
        {
            if (lånadeBöcker.Count < 1)
            {
                Console.WriteLine("Du har inga lånade böcker att återlämna, tryck för att gå tillbaka till meny");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            Console.WriteLine("Utlånade böcker:");
            foreach (var Bok in lånadeBöcker)
            {
                Bok.listaBok();
            }

            bool repris = true;
            while (repris)
            {
                Console.WriteLine("\nVilken bok vill du återlämna?");
                string input = Console.ReadLine();

                var bok = lånadeBöcker.FirstOrDefault(b => b.Titel.StartsWith(input, StringComparison.OrdinalIgnoreCase));

                if (bok != null)
                {
                    while (true)
                    {
                        Console.WriteLine($"Vill du återlämna {bok.Titel}? (Ja/Nej)");
                        string lån = Console.ReadLine();
                        if ("ja".StartsWith(lån, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"{bok.Titel} återlämnades, tryck för att fortsätta");
                            if (bok.Exemplar > 1)
                            {
                                bok.Exemplar--;
                            }
                            else
                            {
                                lånadeBöcker.Remove(bok); // Tar bort boken från listan med lånade böcker 
                            }

                            bool finns = false;
                            foreach (Bok jämförbok in mittBibliotek)
                            {
                                if (jämförbok.Titel == bok.Titel && jämförbok.Förnamn == bok.Förnamn && jämförbok.Efternamn == bok.Efternamn)
                                {
                                    jämförbok.Exemplar++;
                                    finns = true;
                                }
                            }
                            if (!finns)
                            {
                                mittBibliotek.Add(new Bok(bok.Förnamn, bok.Efternamn, bok.Titel, 1)); // Lägger till boken i listan med lånade böcker
                            }

                            SparaBibliotek();
                            SparaLånadeBöcker();
                            Console.ReadKey();
                            Console.Clear();
                            return;
                        }
                        else if ("nej".StartsWith(lån, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Boken lånades inte ut, tryck för att fortsätta");
                            Console.ReadKey();
                            Console.Clear();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Felaktig input, tryck för att försöka igen");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
                }
                else
                {
                    foreach (Bok Bok in mittBibliotek)
                    {
                        bool finns = false;
                        if (Bok.Titel.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                        {
                            finns = true;
                            Console.WriteLine($"{Bok.Titel} är redan utlånad, tryck för att fortsätta");
                            Console.ReadKey();
                            Console.Clear();
                            return;

                        }
                        if (!finns)
                        {
                            Console.WriteLine($"{Bok.Titel} är inte utlånad");
                            Console.WriteLine("Vill du försöka igen? (ja/nej)");
                            string försökIgen = Console.ReadLine();
                            if ("nej".StartsWith(försökIgen, StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                            else if ("ja".StartsWith(försökIgen, StringComparison.OrdinalIgnoreCase))
                            {
                                repris = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Felaktig input, tryck för att försöka igen");
                                Console.ReadKey();
                                Console.Clear();
                                break;
                            }
                        }
                    }
                }
            }
        }

        static void RedigeraBok()
        {
            // Lista alla böcker i biblioteket
            foreach (var bok in mittBibliotek)
            {
                bok.listaBok();
            }

            Console.WriteLine("\nVilken bok vill du redigera? (ange titel)");
            string input = Console.ReadLine();

            var bokAttRedigera = mittBibliotek.FirstOrDefault(b => b.Titel.StartsWith(input, StringComparison.OrdinalIgnoreCase));

            if (bokAttRedigera != null)
            {
                while (true)
                {
                    Console.WriteLine($"Vill du redigera {bokAttRedigera.Titel}? (ja/nej)");
                    string alt = Console.ReadLine();

                    if ("ja".StartsWith(alt, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Vilken del av boken vill du redigera?");
                        Console.WriteLine("1. Författarens förnamn");
                        Console.WriteLine("2. Författarens efternamn");
                        Console.WriteLine("3. Bokens titel");
                        Console.WriteLine("4. Antal exemplar");
                        string val = Console.ReadLine();

                        switch (val)
                        {
                            case "1":
                                Console.WriteLine($"Nuvarande författarens förnamn: {bokAttRedigera.Förnamn}");
                                Console.Write("Ange nytt förnamn (lämna tomt för att behålla det gamla): ");
                                string nyttFörnamn = Console.ReadLine();
                                if (!string.IsNullOrEmpty(nyttFörnamn))
                                {
                                    bokAttRedigera.Förnamn = nyttFörnamn;
                                }
                                break;

                            case "2":
                                Console.WriteLine($"Nuvarande författarens efternamn: {bokAttRedigera.Efternamn}");
                                Console.Write("Ange nytt efternamn (lämna tomt för att behålla det gamla): ");
                                string nyttEfternamn = Console.ReadLine();
                                if (!string.IsNullOrEmpty(nyttEfternamn))
                                {
                                    bokAttRedigera.Efternamn = nyttEfternamn;
                                }
                                break;

                            case "3":
                                Console.WriteLine($"Nuvarande bokens titel: {bokAttRedigera.Titel}");
                                Console.Write("Ange ny titel (lämna tomt för att behålla det gamla): ");
                                string nyTitel = Console.ReadLine();
                                if (!string.IsNullOrEmpty(nyTitel))
                                {
                                    bokAttRedigera.Titel = nyTitel;
                                }
                                break;

                            case "4":
                                Console.WriteLine($"Nuvarande antal exemplar: {bokAttRedigera.Exemplar}");
                                Console.Write("Ange nytt antal exemplar (lämna tomt för att behålla det gamla): ");
                                string nyttAntalExemplar = Console.ReadLine();
                                if (!string.IsNullOrEmpty(nyttAntalExemplar) && int.TryParse(nyttAntalExemplar, out int nyttExemplar))
                                {
                                    bokAttRedigera.Exemplar = nyttExemplar;
                                }
                                else if (!string.IsNullOrEmpty(nyttAntalExemplar))
                                {
                                    Console.WriteLine("Ogiltigt antal, försök igen.");
                                    Console.ReadKey(); // Vänta på att användaren trycker på en tangent innan de fortsätter
                                    continue; // Hoppa tillbaka till menyn för att ge användaren en ny chans
                                }
                                break;

                            default:
                                Console.WriteLine("Felaktig input, försök igen.");
                                Console.ReadKey(); // Vänta på att användaren trycker på en tangent
                                break;
                        }
                    }
                    else if ("nej".StartsWith(alt, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Boken redigerades inte.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Felaktig input, försök igen.");
                        Console.ReadKey(); // Vänta på att användaren trycker på en tangent
                    }

                    // Spara uppdaterad information till fil
                    SparaBibliotek();
                    break;
                }
            }
            else
            {
                Console.WriteLine("Ingen bok med den titeln hittades.");
                Console.ReadKey(); // Vänta på att användaren trycker på en tangent
            }
        }


        //Hjälpmetoder
        static void TaBortSpecifikBok()
        {
            Console.WriteLine("\nVilken bok vill du ta bort? (ange titel)");
            string input = Console.ReadLine();

            var bok = mittBibliotek.FirstOrDefault(b => b.Titel.StartsWith(input, StringComparison.OrdinalIgnoreCase));

            if (bok != null)
            {
                if (bok.Exemplar > 1)
                {
                    Console.WriteLine($"Det finns {bok.Exemplar} exemplar av {bok.Titel}, hur många exemplar vill du ta bort? (1-{bok.Exemplar})");
                    int alternativ = int.Parse(Console.ReadLine());
                    if (alternativ < 1 || alternativ > bok.Exemplar)
                    {
                        Console.WriteLine("Felaktig input, försök igen");
                        return;
                    }
                    else if (alternativ == bok.Exemplar)
                    {
                        mittBibliotek.Remove(bok);
                        Console.WriteLine($"{bok.Titel} togs bort från biblioteket, tryck för att fortsätta");
                        SparaBibliotek();
                        Console.ReadKey();
                        Console.Clear();
                        return;
                    }
                    else
                    {
                        bok.Exemplar -= alternativ;
                        Console.WriteLine($"{alternativ} exemplar av {bok.Titel} togs bort från biblioteket, tryck för att fortsätta");
                        SparaBibliotek();
                        Console.ReadKey();
                        Console.Clear();
                        return;
                    }

                }

                Console.WriteLine($"Vill du ta bort {bok.Titel} från biblioteket? (ja/nej)");
                string alt = Console.ReadLine();

                if ("ja".StartsWith(alt, StringComparison.OrdinalIgnoreCase))
                {
                    mittBibliotek.Remove(bok);
                    Console.WriteLine($"{bok.Titel} togs bort från biblioteket, tryck för att fortsätta");
                    SparaBibliotek();
                    Console.ReadKey();
                    return;
                }
                else if ("nej".StartsWith(alt, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Boken togs inte bort från biblioteket, tryck för att fortsätta");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                Console.WriteLine("Boken hittades inte, tryck för att fortsätta");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void TaBortFörfattare()
        {
            Console.WriteLine("\nVilken författares böcker vill du ta bort? (ange för eller efternamn)");
            string input = Console.ReadLine();

            List<Bok> matchFörfattare = new List<Bok>();

            foreach (var bok in mittBibliotek)
            {
                if (bok.Förnamn.StartsWith(input, StringComparison.OrdinalIgnoreCase) || bok.Efternamn.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                {
                    matchFörfattare.Add(bok);
                }
            }
            Console.WriteLine("Författarens böcker som hittades:");
            foreach (var bok in matchFörfattare)
            {
                bok.listaBok();
            }

            while (true)
            {
                Console.WriteLine("Vill du ta bort alla dessa böcker? (ja/nej)");
                string alt = Console.ReadLine();

                if ("ja".StartsWith(alt, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var bok in matchFörfattare)
                    {
                        mittBibliotek.Remove(bok);
                    }
                    Console.WriteLine("Böckerna togs bort från biblioteket, tryck för att fortsätta");
                    SparaBibliotek();
                    Console.ReadKey();
                    return;
                }
                else if ("nej".StartsWith(alt, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Böckerna togs inte bort från biblioteket, tryck för att fortsätta");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.WriteLine("Felaktig input, försök igen");
                }
            }
        }

        static void SparaBibliotek()
        {
            using (StreamWriter skrivfil = new StreamWriter("Bibliotek.txt"))
            {
                foreach (var bok in mittBibliotek)
                {
                    skrivfil.WriteLine($"{bok.Förnamn},{bok.Efternamn},{bok.Titel},{bok.Exemplar}");
                }
            }
        }

        static void SparaLånadeBöcker()
            {
                using (StreamWriter skrivfil_L = new StreamWriter("Lånade.txt"))
                {
                    foreach (var bok in lånadeBöcker)
                    {
                        skrivfil_L.WriteLine($"{bok.Förnamn},{bok.Efternamn},{bok.Titel},{bok.Exemplar}");
                    }
                }
            }   
    }
}