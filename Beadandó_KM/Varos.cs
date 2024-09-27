using System;
using System.Collections.Generic;
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
                    palya[i][j] = new Fold("F", 1, 1,i,j);
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
                    palya[Xgen][Ygen] = new Barrikad("X", 1, 1, Xgen, Ygen);
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
                    palya[Xgen][Ygen] = new Aranyrog("A", 1, 1, Xgen, Ygen);
                    lerakottrogok++;
                }
            }
        }

        public void banditageneral()
        {
            int lerakottbanditak = 0;
            Random r = new Random();

            while (lerakottbanditak != 15)
            {
                int Xgen = r.Next(0, x);
                int Ygen = r.Next(0, y);

                if (palya[Xgen][Ygen] is Fold && tavolvane(Xgen, Ygen))
                {
                    Bandita bandita = new Bandita("B", 100, 1, 0,Xgen,Ygen);
                    palya[Xgen][Ygen] = bandita;
                    lerakottbanditak++;
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

        public void banditaleptet()
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

        public void sheriffleptet()
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

                        s.sFFeltolt();
                        s.furkesz(ref palya);
                        s.sFAdatElhelyez();
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
                    palya[Xgen][Ygen] = new Whiskey("W", 50, 1,Xgen,Ygen);
                    lerakottwhiskey++;
                }
            }
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
                    palya[Xgen][Ygen] = new Varoshaza("H", 1, 1,Xgen,Ygen);
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

        public int parbaj(Bandita b, Sheriff s)
        {
            // 0 == > sheriff nyert de nem szerzett rögöt
            // 1-3 == > sheriff nyert és szerzett 1-3 rögöt
            // -1 == > sheriff meghalt
            return 0;
        }
        public void szimulal(Sheriff s)
        {
            Console.Clear();
            Console.WriteLine("\x1b");
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    string nev;

                    nev = palya[i][j].nev;
                    if (nev == "F") //föld
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else if (nev == "S") //sheriff
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    else if (nev == "B") //Bandita
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    else if (nev == "W") //Whiskey
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                    }
                    else if (nev == "X") // Barrikád
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (nev == "A") // Arany
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                    }
                    else if (nev == "H") // Városháza
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                    }
                    else
                    {
                        Console.ResetColor();
                    }

                    Console.Write("  ");
                }

                Console.ResetColor();
                Console.WriteLine();
            }

            Console.WriteLine(s.ToString());
            Console.WriteLine("Felszedett aranyrögök száma: "+felszedettRogok);
            Console.WriteLine("Banditáknál lévő rögök száma: "+ banditaknalLevoRogokSzama());
        }
    }
}
