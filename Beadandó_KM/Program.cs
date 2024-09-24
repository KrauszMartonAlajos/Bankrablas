using System;
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
            Sheriff MatyiZok = new Sheriff("S", 100, r.Next(20,36));
            palya.barrikadgeneral(); //jo
            palya.aranyroggeneral(); //jo
            palya.sherifflerak(MatyiZok);//jo
            palya.varoshazalerak(); //jo
            palya.whiskeygeneral(); //jo
            palya.banditageneral(); //jo
            palya.szimulal(MatyiZok);
            palya.szimulal(MatyiZok);
            Thread.Sleep(2000); // Block for 2 seconds
            palya.banditalep();

            palya.szimulal(MatyiZok);
            Thread.Sleep(2000); // Block for 2 seconds
            palya.banditalep();

            Thread.Sleep(2000); // Block for 2 seconds
            palya.szimulal(MatyiZok);

            Thread.Sleep(2000); // Block for 2 seconds
            palya.banditalep();

            Thread.Sleep(2000); // Block for 2 seconds
            palya.szimulal(MatyiZok);

            Console.ReadKey();
        }
    }
}
