using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConcurrentDictionary
{
    class Program
    {
        public static readonly List<string> AllShirtNames = new List<string> { "RickAndMorty", "ChipAndDale", "MarmaidTheCyborg", "StarCraft", "Genius" };

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

            StaffLogsForBonuses staffLogs = new StaffLogsForBonuses();
            ToDoQueue toDoQueue = new ToDoQueue(staffLogs);

            SalesPerson[] people = { new SalesPerson ("Robert"), new SalesPerson ("Emma"), new SalesPerson ("Arnold"), new SalesPerson ("Cameron") };

            StockController controller = new StockController(toDoQueue);
            TimeSpan workDay = new TimeSpan(0, 0, 1);

            Task t1 = Task.Run(() => people[0].Work(controller, workDay));
            Task t2 = Task.Run(() => people[0].Work(controller, workDay));
            Task t3 = Task.Run(() => people[0].Work(controller, workDay));
            Task t4 = Task.Run(() => people[0].Work(controller, workDay));

            Task bonusLogger = Task.Run(() => toDoQueue.MonitorAndLogTrates());
            Task bonusLogger2 = Task.Run(() => toDoQueue.MonitorAndLogTrates());

            Task.WaitAll(t1, t2, t3, t4);
            toDoQueue.CompleteAdding();
            Task.WaitAll(bonusLogger, bonusLogger2);

            controller.DisplayStatus();
            staffLogs.DisplayReport(people);
            Console.ReadKey();
        }
    }
}
