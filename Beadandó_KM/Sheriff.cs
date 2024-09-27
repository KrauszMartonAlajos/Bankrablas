using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadandó_KM
{
    public class Sheriff : VarosElem
    {
        public string nev;
        public int hp;
        public int sebzes;
        public int x, y;     

        public Sheriff(string nev, int hp, int sebzes,int x,int y) : base(nev, hp, sebzes,x,y)
        {
            this.nev = nev; 
            this.hp = hp;   
            this.sebzes = sebzes;
            this.x = x;
            this.y = y;
        }

        public List<List<VarosElem>> sheriffFelfedezett = new List<List<VarosElem>>();

        public void sFFeltolt()
        {
            for (int i = 0; i < 25; i++)
            {
                List<VarosElem> temp = new List<VarosElem>();
                for (int j = 0; j < 25; j++)
                {
                    temp.Add(null);
                }
                sheriffFelfedezett.Add(temp);
            }
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
                    temp.Add(palya[i][j]);
                }
                mitlat.Add(temp);
            }
        }

        public bool mozdult = false;

        public void sFAdatElhelyez()
        {
            for (int i = 0; i < mitlat.Count; i++)
            {
                for (int j = 0; j < mitlat[i].Count; j++)
                {
                    var elem = mitlat[i][j];

                    sheriffFelfedezett[elem.x][elem.y] = elem;

                }
            }
            sheriffFelfedezett[0][0] = null;
            sheriffFelfedezett[x][y] = this;

            //Console.WriteLine("------------------------------------------------");
            //for (int i = 0; i < sheriffFelfedezett.Count; i++)
            //{
            //    for (int j = 0; j < sheriffFelfedezett[i].Count; j++)
            //    {
            //        if (sheriffFelfedezett[i][j] != null)
            //        {
            //            Console.Write(sheriffFelfedezett[i][j].nev);
            //        }
            //        else
            //        {
            //            Console.Write("X");
            //        }
            //    }
            //    Console.WriteLine();
            //}
            Console.WriteLine("------------------------------------------------");
            for (int i = 0; i < mitlat.Count; i++)
            {
                for (int j = 0; j < mitlat[i].Count; j++)
                {
                    if (mitlat[i][j] != null)
                    {
                        Console.Write(mitlat[i][j].nev);
                    }
                    else
                    {
                        Console.Write("X");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine(x + "|" + y);
        }
    }
}
