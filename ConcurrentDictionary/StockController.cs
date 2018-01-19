using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace ConcurrentDictionary
{
    internal class StockController
    {
        ConcurrentDictionary<string, int> _stock = new ConcurrentDictionary<string, int>();
        int _totalQuantityBought;
        int _totalQuantitySold;
        ToDoQueue _toDoQueue;

        public StockController(ToDoQueue bonusCalculator)
        {
            this._toDoQueue = bonusCalculator;
        }

        public void BuyStock(SalesPerson person, string item, int quantity) // note this code is not atomic(
        {
            _stock.AddOrUpdate(item, quantity, (key, oldValue) => oldValue + quantity);
            Interlocked.Add(ref _totalQuantityBought, quantity); // race conditions can appear here(?) But in our case doesn`t matter as no other code is checking these values at this time
            _toDoQueue.AddTrade(new Trade(person, -quantity)); // for producer-consumer scrnario
        }

        public bool TrySellItem(SalesPerson person, string item)
        {
            bool success = false;
            int newStockLevel = _stock.AddOrUpdate(item,
                (itemName) => { success = false; return 0; },
                (itemName, oldValue) =>
                {
                    if (oldValue == 0)
                    {
                        success = false;
                        return 0;
                    }
                    else
                    {
                        success = true;
                        return oldValue - 1;
                    }
                });
            if (success)
            {
                Interlocked.Increment(ref _totalQuantitySold);
                _toDoQueue.AddTrade(new Trade(person, 1));
            }
               
            return success;
        }

        // Here we split one operation into two independent ones
        // so we have simpler code
        // but this method could have worse perfomance in some cases
        // stock level could be negative
        public bool TrySellItem2(SalesPerson person, string item)
        {
            int newStockLevel = _stock.AddOrUpdate(item, -1, (key, oldValue) => oldValue - 1);
            if (newStockLevel < 0)
            {
                _stock.AddOrUpdate(item, 1, (key, oldValue) => oldValue + 1);
                return false;
            }
            else
            {
                Interlocked.Increment(ref _totalQuantitySold);
                _toDoQueue.AddTrade(new Trade(person, 1));
                return true;
            }
        }

        internal void DisplayStatus()
        {
            int totalStock = _stock.Values.Sum(); // .Values is not good for perfomance as it basically needs to block all the threads trying to get access to conc.dict.
            System.Console.WriteLine("\r\nBought = " + _totalQuantityBought);
            System.Console.WriteLine("Sold = " + _totalQuantitySold);
            System.Console.WriteLine("Stock = " + totalStock);
            int error = totalStock + _totalQuantitySold - _totalQuantityBought;
            if (error == 0)
                System.Console.WriteLine("Stock levels match");
            else
                System.Console.WriteLine("Error on stock level: " + error);

            System.Console.WriteLine();
            System.Console.WriteLine("Stock level by item");
            foreach (string itemName in Program.AllShirtNames) 
            {
                int stockLevel = _stock.GetOrAdd(itemName, 0); // see alternative below
                System.Console.WriteLine($"{itemName,-30}: {stockLevel}");

                #region Alternative way
                //int stockLevel;
                //bool success = _stock.TryGetValue(itemName, out stockLevel);
                //if (!success)
                //    stockLevel = 0;
                #endregion
            }
        }
    }
}
