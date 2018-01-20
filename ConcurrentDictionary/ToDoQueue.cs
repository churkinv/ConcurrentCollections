using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ConcurrentDictionary
{
    public class ToDoQueue
    {
        private readonly BlockingCollection<Trade> _queue = new BlockingCollection<Trade>(new ConcurrentQueue<Trade>()); // it is npt necessary to pas a collection to a constructor, 
                                                                                                                         //as it does it implicitly, but we decided to do so explicitly
        //private bool _workingDayComplete = false; commented after implementing BlockingCollection
        private readonly StaffLogsForBonuses _staffLogsForBonuses;

        public ToDoQueue(StaffLogsForBonuses staffResults)
        {
            _staffLogsForBonuses = staffResults;
        }

        public void AddTrade(Trade trade)
        {
            _queue.Add(trade); // used to be Enqueue, but bl.coll. is using the same termonology as conc. bag,
                               //so we replaced it with Add
        }

        public void CompleteAdding()
        {
            //_workingDayComplete = true;
            _queue.CompleteAdding(); // for purpose to tell BlockingCollection that we are done
        }

        public void MonitorAndLogTrates()
        {
            #region old version, with BlockingCollection, but in this case we do not really take an advantage of it, see new version below
            // The code is the same as without using BlockingCollection, see comments below for diff.
            //while (true)
            //{
            //    Trade nextTrade;
            //    bool done = _queue.TryTake(out nextTrade);// used to be TryDequeue, but bl.coll. is using the same termonology as conc. bag,
            //                                              //so we replaced it with TryTake
            //    if (done)
            //    {

            //        _staffLogsForBonuses.ProcessTrade(nextTrade);
            //        Console.WriteLine("Processing transaction from " + nextTrade.Person.Name);
            //    }
            //    else if (_workingDayComplete)
            //    {
            //        Console.WriteLine("No more sales to log - exiting");
            //        return;
            //    }
            //    else
            //    {
            //        Console.WriteLine("No transaction availible");
            //        Thread.Sleep(500);
            //    }
            #endregion

            while (true)
            {
                try
                {
                    Trade nextTransaction = _queue.Take();  // of no item in the collection, this method will wait until an item becomes available
                                                            // as result we don`t have to worry if collection is empty.
                    _staffLogsForBonuses.ProcessTrade(nextTransaction);
                    Console.WriteLine("Processing transaction from " + nextTransaction.Person.Name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message); // we get an InvalidOperationException if no more items are expected, how do we know? See method CompleteAdding()
                    return;
                }
            }

        }
    }
}
