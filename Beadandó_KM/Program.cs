using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Remoting.Contexts;

namespace Beadandó_KM
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Varos palya = new Varos(25, 25);
            bool nyertE = false;
            palya.palyaFeltolt();
            Random r = new Random();
            Sheriff MatyiZok = new Sheriff("S", 100, r.Next(20,36),0,0,true);
            palya.barrikadgeneral(); //jo
            palya.aranyroggeneral(); //jo
            palya.sherifflerak(MatyiZok);//jo
            palya.varoshazalerak(); //jo
            palya.whiskeygeneral(); //jo
            palya.banditageneral(); //jo
            int futasokszama = 0;
            bool fut = true;
            while (fut)
            {
                futasokszama++;
                palya.szimulal(MatyiZok);
                palya.seriffKeres();
                if (futasokszama % 2 == 0)
                { 
                    palya.banditaleptet(ref fut);               
                }
                palya.banditaKeres();
                palya.whiskeyVisszaTolt();  
                palya.whiskeySzamlalo();
                palya.banditaSzamlalo();
                palya.aranyrogSzamlalo();
                palya.sheriffleptet(ref fut, ref nyertE);
                Console.WriteLine("Futások száma: "+futasokszama);
                Thread.Sleep(100); 
            }
            if (nyertE)
            {
                eredmenyLog("eredmenyek.txt", "Futások száma: " + futasokszama + " Győztes: Sheriff");
            }
            else
            {
                eredmenyLog("eredmenyek.txt", "Futások száma: " + futasokszama + " Győztes: Bandita");
            }
            statisztikaLog("eredmenyek.txt","statisztika.txt");
            Console.ReadKey();
        }
        public static void eredmenyLog(string filenev, string szoveg)
        {
            using (StreamWriter w = new StreamWriter(filenev, append: true))
            {
                w.WriteLine(szoveg);
            }
        }
        public static void statisztikaLog(string filenev, string filenev2)
        {
            List<string> sorok = new List<string>();

            using (StreamReader r = new StreamReader(filenev))
            {
                string sor;

                while ((sor = r.ReadLine()) != null)
                {
                    sorok.Add(sor);
                }
            }

            List<int> futasszamok = new List<int>();
            List<string> nyertesek = new List<string>();
            for (int i = 0; i < sorok.Count; i++)
            {
                futasszamok.Add(Convert.ToInt32(sorok[i].Split(' ')[2]));
                nyertesek.Add(sorok[i].Split(' ')[4]);
            }
            double atlag = futasszamok.Sum()/futasszamok.Count;
            int sheriffCount = 0;

            for (int i = 0; i < nyertesek.Count; i++)
            {
                if (nyertesek[i] == "Sheriff")
                {
                    sheriffCount++;
                }
            }

            double winrate = (double)sheriffCount / nyertesek.Count * 100;

            using (StreamWriter writer = new StreamWriter(filenev2, false))
            {
                writer.Write("Átlagos futás szám: "+atlag+"\n"+"Sheriff győzelmi arány: "+ winrate +"%");
            }
        }
    }
}
