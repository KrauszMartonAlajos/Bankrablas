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
        public int rogok;
        
        public Bandita(string nev, int hp, int sebzes, int rogok) : base(nev, hp, sebzes)
        {
            this.nev = nev;
            this.hp = hp;
            this.sebzes = sebzes;
            this.rogok = rogok;
        }
        //public List<List<VarosElem>> lepes(List<List<VarosElem>> palya)
        //{


        //    return null;
        //}
    }
}
