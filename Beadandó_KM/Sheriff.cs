using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Reflection;


namespace Beadandó_KM
{
    public class Sheriff : VarosElem
    {
        public int id = 1;
        public Sheriff(string nev, int hp, int sebzes, int x, int y,bool felfedezett) : base(nev, hp, sebzes, x, y,felfedezett)
        {
            this.nev = nev;
            this.hp = hp;
            this.sebzes = sebzes;
            this.x = x;
            this.y = y;
            this.felfedezett = felfedezett;
        }

        List<List<VarosElem>> mitlat = new List<List<VarosElem>>();
        public void furkesz(ref List<List<VarosElem>> palya)
        {
            mitlat.Clear();

            for (int i = Math.Max(0, x - 1); i <= Math.Min(palya.Count - 1, x + 1); i++)
            {
                List<VarosElem> temp = new List<VarosElem>();
                for (int j = Math.Max(0, y - 1); j <= Math.Min(palya[i].Count - 1, y + 1); j++)
                {
                    palya[i][j].felfedezett = true;
                    temp.Add(palya[i][j]);
                }
                mitlat.Add(temp);
            }
            
        }
        public void mitlatFelfed(ref List<List<VarosElem>> palya)
        {
            for (int i = 0; i < mitlat.Count; i++)
            {
                for (int j = 0; j < mitlat[i].Count; j++)
                {
                    palya[mitlat[i][j].x][mitlat[i][j].y].felfedezett = true;
                }
            }
        }

        public bool mozdult = false;


        public int parbaj(Bandita b)
        {
            Random r = new Random();

            while (this.hp > 0 && b.hp > 0)
            {
                b.hp -= this.sebzes;
                this.hp -= r.Next(4, 15);

                if (b.hp <= 0)
                {
                    return b.rogok;
                }

                if (this.hp <= 0)
                {
                    return -1;
                }
                Thread.Sleep(100);
            }
            return 0;
        }
        public int vhX = -1;
        public int vhY = -1;

        Random r = new Random();

        public int aranyrogSzamlalo(List<List<VarosElem>> palya)
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
            return db;
        }

        public void sherifflep(ref int rogok, ref List<List<VarosElem>> palya, ref bool fut, ref List<Whiskey> ismertWhiskeyk, ref int megoltbanditak, ref bool nyertE)
        {
            if (mozdult)
            {
                return;
            }

            mozdult = true;
            bool felfedezve = mindFelVanEFedezve(ref palya);
            List<Fold> latottFoldek = new List<Fold>();
            for (int i = 0; i < mitlat.Count; i++)
            {
                for (int j = 0; j < mitlat[i].Count; j++)
                {
                    //Aranyrögöt lát kezelése
                    if (mitlat[i][j] is Aranyrog)
                    {
                        int lepX = mitlat[i][j].x;
                        int lepY = mitlat[i][j].y;

                        palya[lepX][lepY] = this;
                        palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y, true);

                        this.x = lepX;
                        this.y = lepY;
                        rogok += 1;
                        return;
                    }
                    else if (this.hp < 0)
                    {
                        Thread.Sleep(1000);
                        fut = false;
                        nyertE = false;
                        Console.Clear();
                        Console.WriteLine("Vesztett a sheriff mert megölték a banditák!");
                    }
                    //Banditát lát kezelése
                    else if (mitlat[i][j] is Bandita)
                    {
                        int eredmeny = parbaj((Bandita)mitlat[i][j]);
                        if (eredmeny == -1)
                        {
                            Thread.Sleep(1000);
                            fut = false;
                            nyertE = false;
                            Console.Clear();
                            Console.WriteLine("Vesztett a sheriff mert megölték a banditák");
                            return;
                        }
                        else
                        {
                            int lepX = mitlat[i][j].x;
                            int lepY = mitlat[i][j].y;

                            palya[lepX][lepY] = this;
                            palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y, true);

                            this.x = lepX;
                            this.y = lepY;
                            rogok += eredmeny;
                            megoltbanditak++;
                            if (megoltbanditak == 3 && rogok == 4 && aranyrogSzamlalo(palya) == 0)
                            {
                                rogok++;
                            }

                        }

                    }

                    //Whiskeyt lát kezelése
                    else if (mitlat[i][j] is Whiskey)
                    {

                        if (this.hp < 51)
                        {
                            int lepX = mitlat[i][j].x;
                            int lepY = mitlat[i][j].y;

                            palya[lepX][lepY] = this;
                            palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y, true);

                            this.x = lepX;
                            this.y = lepY;
                            this.hp += 50;
                            int tobblet = this.hp - 100;
                            if (tobblet > 0)
                            {
                                this.hp -= tobblet;
                            }
                            for (int k = 0; k < ismertWhiskeyk.Count; k++)
                            {
                                if (ismertWhiskeyk[k] == (Whiskey)mitlat[i][j])
                                {
                                    ismertWhiskeyk.RemoveAt(k);
                                }
                            }
                            return;
                        }
                        else
                        {
                            if (!ismertWhiskeyk.Contains(mitlat[i][j]))
                            {
                                ismertWhiskeyk.Add((Whiskey)mitlat[i][j]);
                            }
                        }
                    }

                    //Városházát lát kezelése
                    else if (mitlat[i][j] is Varoshaza)
                    {
                        if (rogok == 5)
                        {
                            int lepX = mitlat[i][j].x;
                            int lepY = mitlat[i][j].y;

                            palya[lepX][lepY] = this;
                            palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y, true);
                            this.x = lepX;
                            this.y = lepY;
                            Thread.Sleep(1000);
                            fut = false;
                            nyertE = true;
                            Console.Clear();
                            Console.WriteLine("Nyert a sheriff össze szedete az összes aranyat!");

                        }

                        vhX = mitlat[i][j].x;
                        vhY = mitlat[i][j].y;
                    }
                    else if (mitlat[i][j] is Fold)
                    {
                        latottFoldek.Add((Fold)mitlat[i][j]);

                    }
                }
            }
            //Város háza felé menés
            if (latottFoldek.Count > 0 && rogok == 5 && vhX != -1 && vhY != -1)
            {
                Fold valasztottFold = null;
                double legkisebbTavolsag = double.MaxValue;

                for (int a = 0; a < latottFoldek.Count; a++)
                {
                    Fold jelenlegiFold = latottFoldek[a];

                    double tavolsag = Math.Sqrt(Math.Pow(jelenlegiFold.x - vhX, 2) + Math.Pow(jelenlegiFold.y - vhY, 2));

                    if (tavolsag < legkisebbTavolsag)
                    {
                        legkisebbTavolsag = tavolsag;
                        valasztottFold = jelenlegiFold;
                    }
                }

                if (valasztottFold != null)
                {
                    palya[valasztottFold.x][valasztottFold.y] = this;
                    palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y, true);

                    this.x = valasztottFold.x;
                    this.y = valasztottFold.y;
                    Thread.Sleep(100);
                }


            }

            //Whiskey felé menés
            else if (latottFoldek.Count > 0 && this.hp < 60 && ismertWhiskeyk.Count > 0)
            {
                Whiskey legkozelebbiWhiskey = null;
                double minWTav = double.MaxValue;

                for (int k = 0; k < ismertWhiskeyk.Count; k++)
                {
                    Whiskey aktWhiskey = ismertWhiskeyk[k];
                    double whiskeyTav = Math.Sqrt(Math.Pow(aktWhiskey.x - this.x, 2) + Math.Pow(aktWhiskey.y - this.y, 2));

                    if (whiskeyTav < minWTav)
                    {
                        minWTav = whiskeyTav;
                        legkozelebbiWhiskey = aktWhiskey;
                    }
                }

                Fold valasztottFold = null;
                double legkisebbTavolsag = double.MaxValue;

                if (legkozelebbiWhiskey != null)
                {
                    for (int a = 0; a < latottFoldek.Count; a++)
                    {
                        Fold jelenlegiFold = latottFoldek[a];

                        double tavolsag = Math.Sqrt(Math.Pow(jelenlegiFold.x - legkozelebbiWhiskey.x, 2) + Math.Pow(jelenlegiFold.y - legkozelebbiWhiskey.y, 2));

                        if (tavolsag < legkisebbTavolsag)
                        {
                            legkisebbTavolsag = tavolsag;
                            valasztottFold = jelenlegiFold;
                        }
                    }
                }

                palya[valasztottFold.x][valasztottFold.y] = this;
                palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y, true);

                this.x = valasztottFold.x;
                this.y = valasztottFold.y;
                Thread.Sleep(100);
            }

            //nem felfedezettek felé menés

            else if (latottFoldek.Count > 0 && !felfedezve)
            {
                int felfedezettlenX = -1;
                int felfedezettlenY = -1;
                double minT = double.MaxValue;

                for (int i = 0; i < palya.Count; i++)
                {
                    for (int j = 0; j < palya[i].Count; j++)
                    {
                        if (palya[i][j] != null && palya[i][j].felfedezett == false)
                        {
                            double tav = Math.Sqrt(Math.Pow(i - this.x, 2) + Math.Pow(j - this.y, 2));

                            if (tav < minT)
                            {
                                minT = tav;
                                felfedezettlenX = i;
                                felfedezettlenY = j;
                            }
                        }
                    }
                }

                Fold valasztottFold = null;
                double minTFold = double.MaxValue;

                if (felfedezettlenX != -1 && felfedezettlenY != -1)
                {
                    for (int k = 0; k < latottFoldek.Count; k++)
                    {
                        Fold aktFold = latottFoldek[k];

                        double foldTav = Math.Sqrt(Math.Pow(aktFold.x - felfedezettlenX, 2) +
                                                        Math.Pow(aktFold.y - felfedezettlenY, 2));

                        if (foldTav < minTFold)
                        {
                            minTFold = foldTav;
                            valasztottFold = aktFold;
                        }
                    }
                }
                palya[valasztottFold.x][valasztottFold.y] = this;
                palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y, true);

                this.x = valasztottFold.x;
                this.y = valasztottFold.y;
            }

            //banditák kergetése
            else if (rogok != 5 && felfedezve)
            {
                Bandita legkozelebbiBandita = null;
                double minBTav = double.MaxValue;

                for (int i = 0; i < palya.Count; i++)
                {
                    for (int j = 0; j < palya[i].Count; j++)
                    {
                        if (palya[i][j] is Bandita)
                        {
                            Bandita aktBandita = (Bandita)palya[i][j];
                            double banditaTav = Math.Sqrt(Math.Pow(aktBandita.x - this.x, 2) + Math.Pow(aktBandita.y - this.y, 2));

                            if (banditaTav < minBTav)
                            {
                                minBTav = banditaTav;
                                legkozelebbiBandita = aktBandita;
                            }
                        }

                    }

                }

                Fold valasztottFold = null;
                double legkisebbTavolsag = double.MaxValue;

                if (legkozelebbiBandita != null)
                {
                    for (int a = 0; a < latottFoldek.Count; a++)
                    {
                        Fold jelenlegiFold = latottFoldek[a];

                        double tavolsag = Math.Sqrt(Math.Pow(jelenlegiFold.x - legkozelebbiBandita.x, 2) + Math.Pow(jelenlegiFold.y - legkozelebbiBandita.y, 2));

                        if (tavolsag < legkisebbTavolsag)
                        {
                            legkisebbTavolsag = tavolsag;
                            valasztottFold = jelenlegiFold;
                        }
                    }
                }

                palya[valasztottFold.x][valasztottFold.y] = this;
                palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y, true);

                this.x = valasztottFold.x;
                this.y = valasztottFold.y;
                Thread.Sleep(100);
            }


            //random lépés
            else if (latottFoldek.Count > 0 && felfedezve)
            {
                Fold valasztottFold = latottFoldek[r.Next(0, latottFoldek.Count)];

                if (palya[valasztottFold.x][valasztottFold.y] is Fold)
                {
                    palya[valasztottFold.x][valasztottFold.y] = this;
                    palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y, true);

                    this.x = valasztottFold.x;
                    this.y = valasztottFold.y;
                }
            }
        }

        public bool mindFelVanEFedezve(ref List<List<VarosElem>> palya)
        {
            int db = 0;
            for (int i = 0; i < palya.Count; i++)
            {
                for (int j = 0; j < palya[i].Count; j++)
                {
                    if (palya[i][j].felfedezett)
                    {
                        db++;
                    }
                }
            }

            if (db == (palya.Count * palya[0].Count))
            { 
                return true;
            }
            else
            { 
                return false; 
            }
        }
    }
}
/*
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠟⠛⠛⠛⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⠿⣿⣿⣿⡿⠋⣠⣶⣿⣿⣿⣷⣄⠙⢿⣿⣿⣿⠛⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠀⠀⠈⠿⡿⠁⣀⣄⡉⠻⣿⣿⠟⠉⢀⣉⠻⠟⠁⠀⣨⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⣶⡄⠀⠀⢀⠀⣀⠀⢀⡃⠀⠀⢀⠀⠱⢠⣶⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠃⢠⣶⣤⡤⠤⢶⣿⣿⣦⣤⣄⣤⣮⠈⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⠀⠘⠛⠋⣀⣀⠈⠛⢛⣋⣩⣌⠻⠿⠆⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⠀⠿⠠⣄⡙⠿⢿⣿⣿⣿⣿⠿⢷⣄⠀⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠃⢀⣶⢸⣿⣿⣤⣤⣤⣴⣶⣶⣿⣿⣿⣆⡘⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠁⢠⣿⡙⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣼⣿⣷⣮⣕⣈⣛⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠟⠁⠀⠀⠀⠙⠃⣤⡙⠛⠻⢿⣿⣿⣿⣿⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⣮⣟⢿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⡿⠟⣋⣍⣩⣤⣄⠀⢀⣤⣶⣶⣄⣈⠙⠂⣀⣀⣭⣥⣴⣶⣾⢡⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⡙⣿⣿⣿⣿
⣿⣿⣿⣿⣿⡿⠁⣤⣾⣿⣿⣿⣿⣿⣶⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣛⣭⣶⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⡜⣿⣿⣿
⣿⣿⣿⣿⠏⢀⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠮⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⢸⣿⣿
⣿⣿⣿⠏⠀⠾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠈⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠁⠘⢻⣿⣿⣿⣿⣿⣿⡌⢿⣿
⣿⣿⣿⠀⢁⡀⠿⡿⠿⠙⠁⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠁⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢏⠔⠀⢀⣣⣄⣉⣿⣿⣿⣿⣿⡌⣿
⣿⣿⡿⠀⠘⢁⠀⠀⠀⠀⠀⢈⠉⠛⢿⣿⣿⣿⣿⣿⣿⣿⡿⢋⡀⠀⠘⣿⢿⣿⡿⠿⠿⡭⠛⠉⠀⠀⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⢸
⣿⣿⢁⣾⣦⠀⢷⣶⣦⠀⠀⠈⠓⢤⡀⠉⠉⠋⠉⠉⠉⠛⠛⣛⣁⠀⠀⠀⢀⣀⠀⣀⡀⠀⠀⠀⠀⠘⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢸
⣿⠇⣼⣿⣿⡇⣼⣿⣿⡇⠀⠀⠀⠀⠀⠀⠈⠁⠀⠀⣠⢶⣿⣿⣿⣷⣦⣤⣄⡀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢸
⣿⠀⣿⣿⡿⢰⣿⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⣀⣀⣤⣤⣶⣾⣿⣿⣿⣿⣿⣶⣟⣀⣠⣄⣠⣴⣶⡀⠀⠙⢿⡏⢿⣿⣿⣿⣿⣿⣿⣾
⡿⠀⢹⡿⢁⣿⣿⣿⣿⠁⣰⡀⠀⠀⠀⠐⠦⡈⢻⣿⣿⣿⣿⣿⡇⢻⣿⣿⣿⣿⣿⣿⣿⣿⡿⠛⠀⠄⡀⠈⠿⠈⠻⣿⣿⣿⣿⡇⣿
⡇⣼⣟⠃⣼⣿⣿⠟⠁⢠⣿⣷⡀⠀⢀⣀⠀⠀⢀⣀⣀⣀⣈⠻⣷⣀⣤⣄⣉⡉⠻⠿⠿⢟⣷⡤⢀⣼⣷⡀⠀⠀⢀⣿⣿⣿⣿⣿⢿
⣠⣿⣿⡄⠉⠉⠁⠀⢠⣾⣿⣿⡇⠀⡺⣿⣷⠀⢸⣿⣿⣿⣿⣿⠈⣽⣿⣿⣿⣿⣯⣴⣶⣶⣿⡇⣾⣿⣿⣿⡄⠀⣤⣿⣿⣿⣿⡿⣸
⣿⣿⣿⣿⣆⠀⠀⣴⣿⣿⣿⣿⡇⠘⣿⣏⢿⡇⠈⠛⠛⢻⣿⣿⠀⢿⣿⣿⣿⣿⣿⣿⣿⣿⡿⢹⣿⣿⣿⣿⢁⣿⣿⣿⣿⣿⣿⡃⢸
⣿⣿⣿⣿⣿⣦⡄⣻⣿⣿⣿⣿⠀⣶⣿⣿⣷⣬⡐⢶⣶⣶⡎⠉⣰⣦⣤⣥⣾⣿⣿⣿⣿⣿⠃⣸⣿⣿⣿⡿⢸⣿⣿⣿⣿⣿⣿⣿⢸
⣿⣿⣿⣿⣿⣿⣄⣻⣿⣿⣿⡿⠀⣿⡟⢻⣿⣿⣿⠾⣾⢿⣿⡟⣿⣿⣿⣿⣿⣿⣿⣿⡿⠯⢠⣿⣿⣿⣿⡇⢸⣿⣿⣿⣿⣿⣿⡟⢸
*/