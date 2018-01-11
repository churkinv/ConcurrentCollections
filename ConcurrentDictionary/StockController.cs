using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentDictionary
{
    public class StockController
    {
        ConcurrentDictionary<string, int> _stock = new ConcurrentDictionary<string, int>();
        int _totalQuantityBought;
        int _totalQuantitySold;


        public void BuyStock(string item, int quantity)
        {
            _stock.AddOrUpdate(item, quantity, (key, oldValue) => oldValue + quantity);
            Interlocked.Add(ref _totalQuantityBought, quantity);
        }

        public bool TrySellItem(string item)
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
                Interlocked.Increment(ref _totalQuantitySold);
            return success;
        }

        internal void DisplayStatus()
        {
            throw new NotImplementedException();
        }
    }
}
