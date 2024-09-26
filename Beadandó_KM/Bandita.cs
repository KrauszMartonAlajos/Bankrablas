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
        public int x, y;

        int[,] iranyok = new int[,] { { 0 , 1 },   // fel
                                      { 0 , -1 },  // le
                                      { -1 , 0 },  // bal
                                      { 1 , 0 },   // jobb
                                      { -1 , 1 },  // bal-fel
                                      { 1 , 1 },   // jobb-fel
                                      { -1 , -1 }, // bal-le
                                      { 1 , -1 }}; //jobb-le

        List<List<VarosElem>> mitlat = new List<List<VarosElem>>();
        public void furkesz(ref List<List<VarosElem>> palya)
        {
            //át kell írni 3*3masra

            mitlat.Clear();
            //kapjon vissza egy n*n es VárosElem mátrixot ami megfelel a látőkörének az adott mezőről és abban hozzon döntést hogy merre lép
            //marad a simán vissza adott koordináta de nem lesz teljesen random hanem így már "okosan" lép
            for (int i = Math.Max(0, x - 3); i < Math.Min(palya.Count, x + 3); i++)
            {
                List<VarosElem> temp = new List<VarosElem>();
                for (int j = Math.Max(0, y - 3); j < Math.Min(palya[i].Count, y + 3); j++)
                {
                    temp.Add(palya[i][j]);
                }
                mitlat.Add(temp);
            }
            Console.WriteLine();
            Console.WriteLine("X: " + x + " Y: " + y);
            Console.WriteLine(mitlat.Count()*mitlat[0].Count());
        }



        public void banditalep()
        {

            Random r = new Random();
            //itt kell egy adott koordinátát vissza adni random

        }

        public Bandita(string nev, int hp, int sebzes, int rogok, int x ,int y) : base(nev, hp, sebzes)
        {
            this.nev = nev;
            this.hp = hp;
            this.sebzes = sebzes;
            this.rogok = rogok;
            this.x = x;
            this.y = y;
        }

    }
}
