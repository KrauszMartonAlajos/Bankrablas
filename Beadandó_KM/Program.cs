﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Beadandó_KM
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Varos palya = new Varos(25, 25);

            palya.palyaFeltolt();
            Random r = new Random();
            Sheriff MatyiZok = new Sheriff("S", 100, r.Next(20,36),0,0);
            palya.barrikadgeneral(); //jo
            palya.aranyroggeneral(); //jo
            palya.sherifflerak(MatyiZok);//jo
            palya.varoshazalerak(); //jo
            palya.whiskeygeneral(); //jo
            palya.banditageneral(); //jo
            int futasokszama = 0;
            while (true)
            {

                futasokszama++;
                palya.szimulal(MatyiZok);
                palya.seriffKeres();
                palya.sheriffleptet();
                if (futasokszama % 2 == 0)
                { 
                    palya.banditaleptet();               
                }
                Console.WriteLine("Futások száma: "+futasokszama);
                Thread.Sleep(1000); 
            }
            Console.ReadKey();
        }
    }
}
