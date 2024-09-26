using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadandó_KM
{
    public class VarosElem
    {
        public string nev;
        public int hp;
        public int sebzes;
        public int x, y;


        public VarosElem(string nev, int hp, int sebzes, int x, int y)
        {
            this.nev = nev;
            this.hp = hp;
            this.sebzes = sebzes;
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return String.Format(nev + " Hp: " + Convert.ToString(hp) + " Dmg: " + Convert.ToString(sebzes));
        }
    }
}
