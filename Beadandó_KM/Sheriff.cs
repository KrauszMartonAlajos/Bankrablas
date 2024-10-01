using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
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
        
        public void sherifflep(ref int rogok, ref List<List<VarosElem>> palya, ref bool fut, ref List<Whiskey> ismertWhiskeyk, ref int megoltbanditak)
        {
            if (mozdult)
            {
                return;
            }

            mozdult = true;

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
                        fut = false;
                        Console.Clear();
                        Console.WriteLine("Vesztett a sheriff mert megölték a banditák!");
                    }

                    else if (mitlat[i][j] is Bandita)
                    {

                        if (this.hp < 60 && ismertWhiskeyk.Count != 0)
                        {

                            int JelenX = this.x;
                            int JelenY = this.y;

                            int CelX = -1;
                            int CelY = -1;
                            double closestDistance = double.MaxValue;

                            for (int k = 0; k < ismertWhiskeyk.Count; k++)
                            {
                                var whiskey = ismertWhiskeyk[k];
                                int whiskeyX = whiskey.x;
                                int whiskeyY = whiskey.y;

                                double distance = Math.Sqrt(Math.Pow(whiskeyX - JelenX, 2) + Math.Pow(whiskeyY - JelenY, 2));

                                if (distance < closestDistance)
                                {
                                    closestDistance = distance;
                                    CelX = whiskeyX;
                                    CelY = whiskeyY;
                                }
                            }

                            if (CelX != -1 && CelY != -1)
                            {
                                int deltaX = CelX - JelenX;
                                int deltaY = CelY - JelenY;
                                if (Math.Abs(deltaX) > Math.Abs(deltaY))
                                {
                                    if (deltaX > 0)
                                    {
                                        this.x++;
                                    }
                                    else
                                    {
                                        this.x--;
                                    }
                                }
                                else
                                {
                                    if (deltaY > 0)
                                    {
                                        this.y++;
                                    }
                                    else
                                    {
                                        this.y--;
                                    }
                                }
                                palya[JelenX][JelenY] = new Fold("F", 1, 1, JelenX, JelenY, true);
                                palya[this.x][this.y] = this;
                            }
                        }

                        else
                        {
                            int eredmeny = parbaj((Bandita)mitlat[i][j]);
                            if (eredmeny == -1)
                            {
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
                                if (megoltbanditak == 3 && rogok == 4)
                                {
                                    rogok++;
                                }

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
                            fut = false;
                            Console.Clear();
                            Console.WriteLine("Nyert a sheriff össze szedett rögök: " + rogok);

                        }

                        vhX = mitlat[i][j].x;
                        vhY = mitlat[i][j].y;
                    }
                    else if (rogok == 5 && vhX != -1 && vhY != -1)
                    {
                        int JelenX = this.x;
                        int JelenY = this.y;

                        int CelX = vhX;
                        int CelY = vhY;

                        int IranyX = CelX - JelenX;
                        int IranyY = CelY - JelenY;

                        if (Math.Abs(IranyX) > Math.Abs(IranyY))
                        {
                            if (IranyX > 0)
                            {
                                this.x++;
                            }
                            else
                            {
                                this.x--;
                            }
                        }
                        else
                        {

                            if (IranyY > 0)
                            {
                                this.y++;
                            }
                            else
                            {
                                this.y--;
                            }
                        }
                        palya[JelenX][JelenY] = new Fold("F", 1, 1, JelenX, JelenY, true);
                        palya[this.x][this.y] = this;
                        fut = false;
                        Console.Clear();
                        Console.WriteLine("Nyert a sheriff össze szedett rögök: " + rogok);
                    }

                    else if (mitlat[i][j] is Fold)
                    {
                        latottFoldek.Add((Fold)mitlat[i][j]);

                    }
                }
            }
            if (latottFoldek.Count > 0)
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
    }
}
