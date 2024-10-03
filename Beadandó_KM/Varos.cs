using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Beadandó_KM
{
    public class Varos
    {
        public List<List<VarosElem>> palya;
        public int x;
        public int y;
        public int felszedettRogok = 0;
        int[,] iranyok = new int[,] { { 0 , 1 },   // fel
                                      { 0 , -1 },  // le
                                      { -1 , 0 },  // bal
                                      { 1 , 0 },   // jobb
                                      { -1 , 1 },  // bal-fel
                                      { 1 , 1 },   // jobb-fel
                                      { -1 , -1 }, // bal-le
                                      { 1 , -1 }}; //jobb-le


        public Varos(int rows, int cols)
        {
            x = rows;
            y = cols;
            palya = new List<List<VarosElem>>(rows);
            for (int i = 0; i < rows; i++)
            {
                palya.Add(new List<VarosElem>(cols));
                for (int j = 0; j < cols; j++)
                {
                    palya[i].Add(null);
                }
            }
        }

        public void palyaFeltolt()
        {
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    palya[i][j] = new Fold("F", 1, 1,i,j,false);
                }
            }
        }

        public void barrikadgeneral()
        {
            int lerakottbarrikadok = 0;
            Random r = new Random();
            int randomBarikadDB = r.Next(25, 51);
            while (lerakottbarrikadok != randomBarikadDB) 
            {
                int Xgen = r.Next(0, x);
                int Ygen = r.Next(0, y);
                if (!(palya[Xgen][Ygen] is Barrikad))
                {
                    palya[Xgen][Ygen] = new Barrikad("X", 1, 1, Xgen, Ygen, false);
                    lerakottbarrikadok++;
                }
            }
        }

        public int banditaknalLevoRogokSzama()
        {
            int db = 0;
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya.Count; j++)
                {
                    if (palya[i][j] is Bandita)
                    {
                        Bandita akt_bandita = (Bandita)palya[i][j];
                        db += akt_bandita.rogok;
                    }
                }
            }
            return db;
        }

        public void aranyroggeneral()
        {
           
            int lerakottrogok = 0;
            Random r = new Random();

            while (lerakottrogok != 5) 
            {
                int Xgen = r.Next(0, x);
                int Ygen = r.Next(0, y);

                if (palya[Xgen][Ygen] is Fold && tavolvane(Xgen, Ygen))
                {
                    palya[Xgen][Ygen] = new Aranyrog("A", 1, 1, Xgen, Ygen,false);
                    lerakottrogok++;
                }
            }
        }

        public void banditageneral()
        {
            int lerakottbanditak = 0;
            Random r = new Random();
            int id = 0;
            while (lerakottbanditak != 4) 
            {
                int Xgen = r.Next(0, x);
                int Ygen = r.Next(0, y);
                
                if (palya[Xgen][Ygen] is Fold && tavolvane(Xgen, Ygen))
                {
                    Bandita bandita = new Bandita("B", 100, 1, 0,Xgen,Ygen,id,false);
                    palya[Xgen][Ygen] = bandita;
                    lerakottbanditak++;
                    id++;
                }

            }
        }

        public void banditaInfo(int id)
        { 
            for (int i = 0; i < palya.Count; i++) 
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j] is Bandita)
                    {
                        Bandita temp = (Bandita)palya[i][j];
                        if (temp.id == id)
                        {
                            Console.WriteLine(temp.ToString());
                        }

                    }
                }
            }
        }

        public void SheriffInfo(int id)
        {
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j] is Sheriff)
                    {
                        Sheriff temp = (Sheriff)palya[i][j];
                        if (temp.id == id)
                        {
                            Console.WriteLine(temp.ToString());
                        }

                    }
                }
            }
        }

        public void seriffKeres()
        {
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j] is Sheriff)
                    {
                        palya[i][j].x = i;
                        palya[i][j].y = j;
                    }
                }
            }
        }
        public void banditaKeres()
        {
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j] is Bandita)
                    {
                        palya[i][j].x = i;
                        palya[i][j].y = j;
                    }
                }
            }
        }

        public void banditaleptet(ref bool fut)
        {           
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j] is Bandita)
                    {
                        ((Bandita)palya[i][j]).mozdult = false;
                    }
                }
            }
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j] is Bandita)
                    {
                        Bandita b = (Bandita)palya[i][j];
                        b.furkesz(ref palya);
                        b.banditalep(ref palya);
                    }
                }
            }
        }

        

        public void sheriffFelfedez(int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (lephetEra(x + i, y + j))
                    {
                        palya[x + i][y + j].felfedezett = true;

                    }
                }
            }
        }
        public bool lephetEra(int x, int y)
        {
            return x >= 0 && x < palya.Count && y >= 0 && y < palya[0].Count;
        }

        public int megoltbanditak = 0;

        public void sheriffleptet(ref bool fut)
        {
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j] is Sheriff)
                    {
                        ((Sheriff)palya[i][j]).mozdult = false;
                    }
                }
            }
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j] is Sheriff)
                    {
                        Sheriff s = (Sheriff)palya[i][j];
                        s.furkesz(ref palya);
                        sheriffFelfedez(s.x, s.y);
                        s.sherifflep(ref felszedettRogok, ref palya, ref fut, ref ismertWhiskeyk, ref megoltbanditak);
                        sheriffFelfedez(s.x, s.y);

                    }
                }
            }
        }

        public void whiskeygeneral()
        {
            int lerakottwhiskey = 0;
            Random r = new Random();

            while (lerakottwhiskey != 3) 
            {
                int Xgen = r.Next(0, x);
                int Ygen = r.Next(0, y);

                if (palya[Xgen][Ygen] is Fold && tavolvane(Xgen, Ygen))
                {
                    palya[Xgen][Ygen] = new Whiskey("W", 50, 1,Xgen,Ygen,false);
                    lerakottwhiskey++;
                }
            }
        }

        List<Whiskey> ismertWhiskeyk = new List<Whiskey>();

        public void whiskeyVisszaTolt()
        {
            int db = 0;
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j] is Whiskey)
                    {
                        db++;
                    }
                }
            }
            Random rand = new Random();

            while (db < 3)
            {
                int randX = rand.Next(0, palya.Count);
                int randY = rand.Next(0, palya[randX].Count);

                if (palya[randX][randY] is Fold)
                {
                    bool elozoallapot = palya[randX][randY].felfedezett;
                    if (elozoallapot)
                    {
                        Whiskey ujW = new Whiskey("W", 50, 1, randX, randY, elozoallapot);
                        palya[randX][randY] = ujW;
                        ismertWhiskeyk.Add(ujW);
                        db++;
                    }
                    else
                    {
                        Whiskey ujW = new Whiskey("W", 50, 1, randX, randY, elozoallapot);
                        palya[randX][randY] = ujW;
                        db++;
                    }
                    
                }
            }
        }


        public void whiskeySzamlalo()
        {
            int db = 0;
            for (int i = 0; i < palya.Count; i++) 
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j] is Whiskey)
                    {
                        db++;
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(db+" db Whiskey van a pályán!");
            Console.ResetColor();
        }

        public void banditaSzamlalo()
        {
            int db = 0;
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j] is Bandita)
                    {
                        db++;
                    }
                }
            }
            Console.WriteLine(db + " db Bandita van a pályán!");
        }

        public void aranyrogSzamlalo()
        {
            int db = 0;
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j] is Aranyrog)
                    {
                        db++;
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(db + " db Aranyrog van a pályán!");
            Console.ResetColor();
        }

        public void sherifflerak(Sheriff s)
        {
            int lerakottseriff = 0;
            Random r = new Random();

            while (lerakottseriff != 1)
            {
                int Xgen = r.Next(0, x);
                int Ygen = r.Next(0, y);
                s.x = Xgen;
                s.y = Ygen; //it lehetett változtatni az értékét
                if (palya[Xgen][Ygen] is Fold && tavolvane(Xgen, Ygen))
                {
                    palya[Xgen][Ygen] = s;
                    lerakottseriff++;
                }
            }
        }

        public void varoshazalerak()
        {
            int lerakottvaroshaza = 0;
            Random r = new Random();

            while (lerakottvaroshaza != 1) 
            {
                int Xgen = r.Next(0, x);
                int Ygen = r.Next(0, y);

                if (palya[Xgen][Ygen] is Fold && tavolvane(Xgen, Ygen))
                {
                    palya[Xgen][Ygen] = new Varoshaza("H", 1, 1,Xgen,Ygen,false);
                    lerakottvaroshaza++;
                }
            }
        }


        public bool tavolvane(int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newX = x + i;
                    int newY = y + j;

                    if (newX >= 0 && newX < palya.Count && newY >= 0 && newY < palya[0].Count)
                    {
                        if (palya[newX][newY] is Barrikad || palya[newX][newY] is Bandita || palya[newX][newY] is Whiskey || palya[newX][newY] is Aranyrog || palya[newX][newY] is Varoshaza)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        public void szimulal(Sheriff s)
        {
            Console.Clear();
            Console.WriteLine("\x1b[2J");
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j].felfedezett)
                    {
                        string nev;
                        nev = palya[i][j].nev;
                        if (nev == "F") //föld
                        {
                            //Console.Write(nev);
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        else if (nev == "S") //sheriff
                        {
                            //Console.Write(nev);
                            Console.BackgroundColor = ConsoleColor.Red;
                        }
                        else if (nev == "B") //Bandita
                        {
                            //Console.Write(nev);
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }
                        else if (nev == "W") //Whiskey
                        {
                            //Console.Write(nev);
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                        }
                        else if (nev == "X") // Barrikád
                        {
                            //Console.Write(nev);
                            Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        }
                        else if (nev == "A") // Arany
                        {
                            //Console.Write(nev);
                            Console.BackgroundColor = ConsoleColor.Yellow;
                        }
                        else if (nev == "H") // Városháza
                        {
                            //Console.Write(nev);
                            Console.BackgroundColor = ConsoleColor.Blue;
                        }
                        else
                        {
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    Console.Write("  ");

                }

                Console.ResetColor();
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s.ToString());

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Megölt banditák száma: " + megoltbanditak);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Felszedett aranyrögök száma: " + felszedettRogok);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Banditáknál lévő rögök száma: " + banditaknalLevoRogokSzama());

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Városháza ismert koordinátái: " + s.vhX + " | " + s.vhY);

            for (int i = 0; i < ismertWhiskeyk.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Whiskey " + (i + 1) + ". koordinátái: (" + ismertWhiskeyk[i].x + "," + ismertWhiskeyk[i].y + ")");
            }

            Console.ResetColor();
        }
    }
}
