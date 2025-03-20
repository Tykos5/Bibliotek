using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.CodeDom;

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
                        Console.WriteLine("Felaktig input försök igen");
                        Thread.Sleep(1000);
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
                using (FileStream fs = File.Create("Bibliotek.txt"))
                {
                    // File is created and closed immediately
                }

                using (StreamWriter skrivfil = new StreamWriter("Bibliotek.txt"))  // Hårdkoda in 13 böcker i biblioteket då den skapas
                {
                    skrivfil.WriteLine("J.K.,Rowling,Harry Potter och de vises sten");
                    skrivfil.WriteLine("J.K.,Rowling,Harry Potter och hemligheternas kammare");
                    skrivfil.WriteLine("J.K.,Rowling,Harry Potter och fången från Azkaban");
                    skrivfil.WriteLine("J.K.,Rowling,Harry Potter och den flammande bägaren");
                    skrivfil.WriteLine("Harper,Lee,To kill a mockingbird");
                    skrivfil.WriteLine("Per Anders,Fogelström,Mina drömmars stad");
                    skrivfil.WriteLine("Jan,Guillou,Ondskan");
                    skrivfil.WriteLine("Astrid,Lindgren,Pippi Långstrump");
                    skrivfil.WriteLine("Astrid,Lindgren,Bröderna Lejonhjärta");
                    skrivfil.WriteLine("Astrid,Lindgren,Emil i Lönneberga");
                    skrivfil.WriteLine("Astrid,Lindgren,Ronja Rövardotter");
                    skrivfil.WriteLine("Astrid,Lindgren,Mio min Mio");
                    skrivfil.WriteLine("Khaled,Hosseini,Flyga drake");
                }
            }
            using (StreamReader läsfil = new StreamReader("Bibliotek.txt"))
            {
                string s;
                while ((s = läsfil.ReadLine()) != null)
                {
                    string[] bokdata = s.Split(',');
                    mittBibliotek.Add(new Bok(bokdata[0], bokdata[1], bokdata[2]));
                }
            }

            lånadeBöcker.Clear();
            if (!File.Exists("Lånade.txt"))
            {
                File.Create("Lånade.txt");  // Skapar filen om den inte finns
            }
            else
            {
                using (StreamReader läsfil_L = new StreamReader("Lånade.txt"))
                {
                    string b;
                    while ((b = läsfil_L.ReadLine()) != null)
                    {
                        string[] bokdata = b.Split(',');
                        lånadeBöcker.Add(new Bok(bokdata[0], bokdata[1], bokdata[2]));
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
                    Console.WriteLine("Felaktig input, försök igen");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
        } 

        static void SökBok()
        {
            bool repris = true;
            while (repris)
            {
                Console.WriteLine("Vilken författare vill du söka efter? (ange författarens för eller efternamn)");
                string input = Console.ReadLine();

                List<Bok> matchNamn = new List<Bok>();  // lista för att hantera matchande författare
                List<Bok> matchNamnLånad = new List<Bok>(); // lista för att hantera matchande författare i lånade böcker

                foreach (var bok in mittBibliotek)
                {
                    if (bok.Förnamn.StartsWith(input, StringComparison.OrdinalIgnoreCase) || bok.Efternamn.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                    {
                        matchNamn.Add(bok);  // lagrar böcker med matchande efternamn
                    }
                }
                foreach (var bok in lånadeBöcker)
                {
                    if (bok.Förnamn.StartsWith(input, StringComparison.OrdinalIgnoreCase) || bok.Efternamn.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                    {
                        matchNamnLånad.Add(bok);  // lagrar böcker med matchande efternamn
                    }
                }
                if (matchNamnLånad.Count < 1 && matchNamn.Count < 1) // Då inga matcher hittas
                {
                    Console.WriteLine("Ingen författare matchar med din sökning:");
                    Console.WriteLine("Vill du försöka igen? (ja/nej)");
                    string försökIgen = Console.ReadLine();
                    if ("ja".StartsWith(försökIgen, StringComparison.OrdinalIgnoreCase))
                    {
                        repris = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Författare som stämmer med din sökning:");

                    foreach (var bok in matchNamn)
                    {
                        bok.listaBok(); // skriver ut böckerna med matchande efternamn
                    }
                    foreach (var bok in matchNamnLånad)
                    {
                        bok.listaBokLånade(); // skriver ut böckerna med matchande efternamn
                    }
                }
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
                            Console.WriteLine($"{bok.Titel} lånades ut");
                            lånadeBöcker.Add(bok); // Lägger till boken i listan med lånade böcker  
                            mittBibliotek.Remove(bok); // Tar bort boken från biblioteket 
                            SparaBibliotek();
                            SparaLånadeBöcker();
                            break;
                        }
                        else if ("nej".StartsWith(lån, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Boken lånades inte ut");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Felaktig input, försök igen");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Boken hittades inte i biblioteket, vill du försöka igen? (ja/nej)");
                    string försökIgen = Console.ReadLine();
                    if ("ja".StartsWith(försökIgen, StringComparison.OrdinalIgnoreCase))
                    {
                        repris = true;
                    }
                }
            }
        }

        static void ÅterlämnaBok()
        {
            if (lånadeBöcker.Count < 1)
            {
                Console.WriteLine("Du har inga lånade böcker att återlämna");
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
                            Console.WriteLine($"{bok.Titel} återlämnades");
                            lånadeBöcker.Add(bok); // Lägger till boken i listan med lånade böcker  
                            mittBibliotek.Remove(bok); // Tar bort boken från biblioteket 
                            SparaBibliotek();
                            SparaLånadeBöcker();
                            return;
                        }
                        else if ("nej".StartsWith(lån, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Boken lånades inte ut");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Felaktig input, försök igen");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Boken är inte utlånad");
                }
            }

        }

        static void RedigeraBok()
        {
            foreach (var bok in mittBibliotek)
            {
                bok.listaBok();
            }

            Console.WriteLine("\nVilken bok vill du redigera? (ange titel)");
            string input = Console.ReadLine();

            foreach (var Bok in mittBibliotek)
            {
                if (Bok.Titel.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                {
                    while (true)
                    {
                        Console.WriteLine($"Vill du redigera {Bok.Titel}? (ja/nej)");
                        string alt = Console.ReadLine();

                        if ("ja".StartsWith(alt, StringComparison.OrdinalIgnoreCase))
                        {

                            Console.WriteLine("Vilken del av boken vill du redigera?");
                            Console.WriteLine("1. Författarens förnamn");
                            Console.WriteLine("2. Författarens efternamn");
                            Console.WriteLine("3. Bokens titel");
                            string val = Console.ReadLine();
                            switch (val)
                            {
                                case "1":
                                    Console.WriteLine("Vad är författarens förnamn?");
                                    Bok.Förnamn = Console.ReadLine();
                                    break;
                                case "2":
                                    Console.WriteLine("Vad är författarens efternamn?");
                                    Bok.Efternamn = Console.ReadLine();
                                    break;
                                case "3":
                                    Console.WriteLine("Vad är bokens titel?");
                                    Bok.Titel = Console.ReadLine();
                                    break;
                                default:
                                    Console.WriteLine("Felaktig input, försök igen");
                                    break;
                            }
                        }
                        else if ("nej".StartsWith(alt, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Boken redigerades inte");
                            Thread.Sleep(1000);
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Felaktig input, försök igen");
                            Thread.Sleep(1000);

                        }
                        SparaBibliotek();
                        return;
                    }
                }
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

                Console.WriteLine($"Vill du ta bort {bok.Titel} från biblioteket? (ja/nej)");
                string alt = Console.ReadLine();

                if ("ja".StartsWith(alt, StringComparison.OrdinalIgnoreCase))
                {
                    mittBibliotek.Remove(bok);
                    Console.WriteLine($"{bok.Titel} togs bort från biblioteket");
                    SparaBibliotek();
                    Thread.Sleep(1000);
                    return;
                }
                else if ("nej".StartsWith(alt, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Boken togs inte bort från biblioteket");
                    Thread.Sleep(1000);
                    return;
                }
            }
            else
            {
                Console.WriteLine("Boken hittades inte.");
                Thread.Sleep(1000);
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
                    Console.WriteLine("Böckerna togs bort från biblioteket");
                    SparaBibliotek();
                    Thread.Sleep(1000);
                    return;
                }
                else if ("nej".StartsWith(alt, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Böckerna togs inte bort från biblioteket");
                    Thread.Sleep(1000);
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
                    skrivfil.WriteLine($"{bok.Förnamn},{bok.Efternamn},{bok.Titel}");
                }
            }
        }

        static void SparaLånadeBöcker()
        {
            using (StreamWriter skrivfil_L = new StreamWriter("Lånade.txt"))
            {
                foreach (var bok in lånadeBöcker)
                {
                    skrivfil_L.WriteLine($"{bok.Förnamn},{bok.Efternamn},{bok.Titel}");
                }
            }
        }
    }
}