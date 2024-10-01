using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadandó_KM
{
    public class Varoshaza : VarosElem
    {
        public string nev;
        public int hp;
        public int sebzes;
        public int x, y;
        public bool felfedezett;

        public Varoshaza(string nev, int hp, int sebzes, int x, int y, bool felfedezett) : base(nev, hp, sebzes, x, y, felfedezett)
        {
            this.nev = nev;
            this.hp = hp;
            this.sebzes = sebzes;
            this.x = x;
            this.y = y;
            this.felfedezett = felfedezett;
        }
    }
}
