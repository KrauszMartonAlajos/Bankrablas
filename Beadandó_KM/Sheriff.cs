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

        public void sherifflep(ref int rogok, ref List<List<VarosElem>> palya, ref bool fut, ref List<Whiskey> ismertWhiskeyk, ref int megoltbanditak)
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
                        Console.Clear();
                        Console.WriteLine("Vesztett a sheriff mert megölték a banditák!");
                    }

                    else if (mitlat[i][j] is Bandita)
                    {
                        int eredmeny = parbaj((Bandita)mitlat[i][j]);
                        if (eredmeny == -1)
                        {
                            Thread.Sleep(1000);
                            fut = false;
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
                    else if (mitlat[i][j] is Whiskey)
                    {

                        if (this.hp < 60)
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
                            Console.Clear();
                            Console.WriteLine("Nyert a sheriff össze szedett rögök: " + rogok);

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
            //Város háza közelítés
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

            //Whiskey közelítés
            else if (latottFoldek.Count > 0 && this.hp < 60 && ismertWhiskeyk.Count > 0)
            {
                Whiskey legkozelebbiWhiskey = null;
                double minWTav = double.MaxValue;

                for (int k = 0; k < ismertWhiskeyk.Count; k++)
                {
                    Whiskey aktWhiskey = ismertWhiskeyk[k];
                    double whiskeyDistance = Math.Sqrt(Math.Pow(aktWhiskey.x - this.x, 2) + Math.Pow(aktWhiskey.y - this.y, 2));

                    if (whiskeyDistance < minWTav)
                    {
                        minWTav = whiskeyDistance;
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
                        Fold currentFold = latottFoldek[k];

                        double foldDistance = Math.Sqrt(Math.Pow(currentFold.x - felfedezettlenX, 2) +
                                                        Math.Pow(currentFold.y - felfedezettlenY, 2));

                        if (foldDistance < minTFold)
                        {
                            minTFold = foldDistance;
                            valasztottFold = currentFold;
                        }
                    }
                }
                palya[valasztottFold.x][valasztottFold.y] = this;
                palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y, true);

                this.x = valasztottFold.x;
                this.y = valasztottFold.y;
            }


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
