using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            var stock = new Dictionary<string, int>()
            {
                {"Rick", 4},
                {"Morty", 3}
            };

            Console.WriteLine($"No. of shirts in stock = {stock.Count}");

            stock.Add("Let`sGetShifty", 6);
            stock["BirdMan"] = 5; // adding by using an indexer

            stock["Let`sGetShifty"] = 12; // up from 6, 6 were bought
            Console.WriteLine($"\r\nstock[Let`sGetShifty] = {stock["Let`sGetShifty"]}");

            stock.Remove("Rick");

            Console.WriteLine("\r\nEnumerating:");
            foreach (var kewValPair in stock)
            {
                Console.WriteLine($"{kewValPair.Key}: {kewValPair.Value}");
            }
            Console.WriteLine($"No. of shirts in stock = {stock.Count}");
            Console.ReadKey();
        }
    }    
}
