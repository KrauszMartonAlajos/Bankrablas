using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadandó_KM
{
    public class Bandita : VarosElem
    {
        public string nev;
        public int hp;
        public int sebzes;

        public Bandita(string nev, int hp, int sebzes) : base(nev, hp, sebzes)
        {
            this.nev = nev;
            this.hp = hp;
            this.sebzes = sebzes;
        }
    }
}
