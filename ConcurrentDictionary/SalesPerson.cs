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
                    //bool success = controller.TrySellItem(itemName);
                    //DisplaySaleAttempt(success, itemName);
                }

            }
        }

        private void DisplaySaleAttempt(bool success, string itemName)
        {
            throw new NotImplementedException();
        }

        private void DisplayPurchase(string itemName, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
