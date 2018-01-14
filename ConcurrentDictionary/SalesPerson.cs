using System;
using System.Threading;

namespace ConcurrentDictionary
{
    public class SalesPerson
    {
        public string Name { get; private set; }

        public SalesPerson(string id)
        {
            this.Name=id;
        }

        internal void Work(StockController controller, TimeSpan workDay)
        {
            Random rand = new Random(Name.GetHashCode());
            DateTime start = DateTime.Now;
            while (DateTime.Now - start < workDay)
            {
                Thread.Sleep(rand.Next(100)); // by commenting this we can get real stress test of our conc. dict, as all transactions are effected almost at the same time.   
                bool buy = (rand.Next(6) == 0); // will choose a number between 0-5, and if 0 we buy, so we sell 5 times more than buy 
                string itemName = Program.AllShirtNames[rand.Next(Program.AllShirtNames.Count)];
                if (buy)
                {
                    int quantity = rand.Next(9) + 1;
                    controller.BuyStock(itemName, quantity);
                    DisplayPurchase(itemName, quantity);
                }
                else
                {
                    bool success = controller.TrySellItem2(itemName); // or try TrySellItem
                    DisplaySaleAttempt(success, itemName);
                }

            }
            Console.WriteLine($"SalesPerson {this.Name}");
        }

        private void DisplaySaleAttempt(bool success, string itemName)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            if (success)
                Console.WriteLine(string.Format("Thread {0}: {1} sold {2}", threadId, this.Name, itemName));
            else
                Console.WriteLine(string.Format("Thread {0}: {1}: Out of stock of {2}", threadId, this.Name, itemName));
        }

        private void DisplayPurchase(string itemName, int quantity)
        {
            Console.WriteLine("Thread {0}: {1} bought {2} of {3}", Thread.CurrentThread.ManagedThreadId, this.Name, quantity, itemName);
        }
    }
}
