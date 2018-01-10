using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentDictionary
{
    class Program
    {
        public static readonly List<string> AllShirtNames = new List<string> {"RickAndMorty", "ChipAndDale", "MarmaidTheCyborg", "StarCraft", "Genius" };

        static void Main(string[] args)
        {
            #region old
            //var stock = new Dictionary<string, int>()
            //{
            //    {"Rick", 4},
            //    {"Morty", 3}
            //};

            //Console.WriteLine($"No. of shirts in stock = {stock.Count}");

            //stock.Add("Let`sGetShifty", 6);
            //stock["BirdMan"] = 5; // adding by using an indexer

            //stock["Let`sGetShifty"] = 12; // up from 6, 6 were bought
            //Console.WriteLine($"\r\nstock[Let`sGetShifty] = {stock["Let`sGetShifty"]}");

            //stock.Remove("Rick");

            //Console.WriteLine("\r\nEnumerating:");
            //foreach (var kewValPair in stock)
            //{
            //    Console.WriteLine($"{kewValPair.Key}: {kewValPair.Value}");
            //}
            //Console.WriteLine($"No. of shirts in stock = {stock.Count}");
            //Console.ReadKey();
            #endregion

            StockController controller = new StockController();
            TimeSpan workDay = new TimeSpan(0, 0, 2);

            Task t1 = Task.Run(() => new SalesPerson("Jonhy").Work(controller, workDay));
            Task t2 = Task.Run(() => new SalesPerson("Cameron").Work(controller, workDay));
            Task t3 = Task.Run(() => new SalesPerson("Shiller").Work(controller, workDay));
            Task t4 = Task.Run(() => new SalesPerson("Emma").Work(controller, workDay));

            Task.WaitAll(t1, t2, t3, t4);
            controller.DisplayStatus();

        }
    }    
}
