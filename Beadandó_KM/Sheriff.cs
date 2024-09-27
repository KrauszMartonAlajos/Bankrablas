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
    }
}
