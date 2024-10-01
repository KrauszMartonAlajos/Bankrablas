using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

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


        //public void sFFeltolt()
        //{
        //    for (int i = 0; i < 25; i++)
        //    {
        //        List<VarosElem> temp = new List<VarosElem>();
        //        for (int j = 0; j < 25; j++)
        //        {
        //            temp.Add(null);
        //        }
        //        sheriffFelfedezett.Add(temp);
        //    }
        //}

        List<List<VarosElem>> mitlat = new List<List<VarosElem>>();
        public void furkesz(ref List<List<VarosElem>> palya)
        {
            mitlat.Clear();

            for (int i = Math.Max(0, x - 1); i <= Math.Min(palya.Count - 1, x + 1); i++)
            {
                List<VarosElem> temp = new List<VarosElem>();
                for (int j = Math.Max(0, y - 1); j <= Math.Min(palya[i].Count - 1, y + 1); j++)
                {
                    
                    temp.Add(palya[i][j]);
                }
                mitlat.Add(temp);
            }
            
        }
        //public void mitlatFelfed(ref List<List<VarosElem>> palya)
        //{
        //    for (int i = 0; i < mitlat.Count; i++)
        //    {
        //        for (int j = 0; j < mitlat[i].Count; j++)
        //        {
        //            palya[mitlat[i][j].x][mitlat[i][j].y].felfedezett = true;
        //        }
        //    }
        //}


        public bool mozdult = false;


        //public List<List<VarosElem>> sheriffFelfedezett = new List<List<VarosElem>>();

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
        
        public void sherifflep(ref int rogok, ref List<List<VarosElem>> palya, ref bool fut, ref List<Whiskey> ismertWhiskeyk)
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
                        palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y,true);

                        this.x = lepX;
                        this.y = lepY;
                        rogok += 1;
                        return;
                    }
                    else if (mitlat[i][j] is Bandita)
                    {

                        if (this.hp < 4)
                        {
                            //megindul a legközelebbi whiskey felé
                        }
                        else 
                        {
                            int eredmeny = parbaj((Bandita)mitlat[i][j]);
                            if (eredmeny == -1) //ez jól működik
                            {
                                fut = false;
                                Console.Clear();
                                Console.WriteLine("Vesztett a sheriff");
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
                        if (rogok >= 4)
                        {
                            int lepX = mitlat[i][j].x;
                            int lepY = mitlat[i][j].y;

                            palya[lepX][lepY] = this;
                            palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y,true);
                            this.x = lepX;
                            this.y = lepY;
                            fut = false;
                            Console.Clear();
                            Console.WriteLine("Nyert a sheriff");

                        }

                        vhX = mitlat[i][j].x;
                        vhY = mitlat[i][j].y;
                    }
                    else if (rogok == 4)
                    { 
                        //itt kell elkezdeni keresni a városházát a meglévő koordináta alapján
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


        //public void mitLataSheriff()
        //{
        //    for (int i = 0; i < sheriffFelfedezett.Count; i++)
        //    {
        //        for (int j = 0; j < sheriffFelfedezett[i].Count; j++)
        //        {
        //            if (sheriffFelfedezett[i][j] != null)
        //            {
        //                Console.Write(sheriffFelfedezett[i][j].nev);
        //            }
        //            else
        //            {
        //                Console.Write("-");
        //            }
        //        }
        //        Console.WriteLine();
        //    }
        //    sheriffFelfedezett.Clear();
        //}
    }
}
