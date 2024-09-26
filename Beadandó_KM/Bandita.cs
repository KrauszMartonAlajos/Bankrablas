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



        public void banditalep(ref List<List<VarosElem>> palya)
        {

            Random r = new Random();
            //itt kell egy adott koordinátát vissza adni random
            Console.WriteLine();
            Console.WriteLine("X: " + x + " Y: " + y);
            Console.WriteLine(mitlat.Count() * mitlat[0].Count());
            for (int i = 0; i < mitlat.Count; i++)
            {
                for (int j = 0; j < mitlat[i].Count; j++)
                {
                    if (mitlat[i][j] is Aranyrog)
                    {
                        int lepX = mitlat[i][j].x;
                        int lepY = mitlat[i][j].y; //kimentjük hova akarunk lépni

                        palya[lepX][lepY] = this; //kivalasztott mezőre tesszük a banditát

                        palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y); //bandita volt helyére földet teszünk

                        this.x = lepX;
                        this.y = lepY; //megadjuk a bandita új helyét

                        this.rogok += 1; //fel vette a rögöt
                    }
                    else if (mitlat[i][j] is Sheriff)
                    {
                        //még nincs kész a methódus
                    }
                    else
                    {
                        List<Fold> latottFoldek = new List<Fold>();
                        if (mitlat[i][j] is Fold)
                        {
                            latottFoldek.Add((Fold)mitlat[i][j]);
                        }
                        Fold valasztottFold = latottFoldek[r.Next(0, latottFoldek.Count)];
                        //valami miatt 0 lesz a hossz

                        palya[valasztottFold.x][valasztottFold.y] = this;
                        palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y); //bandita volt helyére földet teszünk
                        this.x = valasztottFold.x;
                        this.y = valasztottFold.y;
                    }
                }
            }
        }

        public Bandita(string nev, int hp, int sebzes, int rogok, int x ,int y) : base(nev, hp, sebzes, x, y)
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
