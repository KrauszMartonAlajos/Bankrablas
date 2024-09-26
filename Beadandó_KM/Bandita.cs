﻿using System;
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
        public Bandita(string nev, int hp, int sebzes, int rogok, int x, int y) : base(nev, hp, sebzes, x, y)
        {
            this.nev = nev;
            this.hp = hp;
            this.sebzes = sebzes;
            this.rogok = rogok;
            this.x = x;
            this.y = y;
        }

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

        public static Random random = new Random();
        public bool mozdult = false;


        public void banditalep(ref List<List<VarosElem>> palya)
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
                        palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y);

                        this.x = lepX;
                        this.y = lepY;
                        this.rogok += 1;
                        return;
                    }
                    else if (mitlat[i][j] is Sheriff)
                    {
                        // sheriffel itt találkozik
                    }
                    else if (mitlat[i][j] is Fold)
                    {
                        latottFoldek.Add((Fold)mitlat[i][j]);
                    }
                }
            }
            if (latottFoldek.Count > 0)
            {
                Fold valasztottFold = latottFoldek[random.Next(0, latottFoldek.Count)];

                if (palya[valasztottFold.x][valasztottFold.y] is Fold)
                {
                    palya[valasztottFold.x][valasztottFold.y] = this;
                    palya[this.x][this.y] = new Fold("F", 1, 1, this.x, this.y);

                    this.x = valasztottFold.x;
                    this.y = valasztottFold.y;
                }
            }

        }

        

    }
}
