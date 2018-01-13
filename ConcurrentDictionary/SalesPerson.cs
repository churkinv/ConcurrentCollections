using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                Thread.Sleep(rand.Next(100));
                bool buy = (rand.Next(6) == 0);
                string itemName = Program.AllShirtNames[rand.Next(Program.AllShirtNames.Count)];
                if (buy)
                {
                    int quantity = rand.Next(9) + 1;
                    controller.BuyStock(itemName, quantity);
                    DisplayPurchase(itemName, quantity);
                }
                else
                {
                    bool success = controller.TrySellItem(itemName);
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
