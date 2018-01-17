namespace ConcurrentDictionary
{
    public class Trade
    {
        public SalesPerson Person { get; private set; }

        //QuantitySold is negative if thr trade was a purchase
        public int QuantitySold { get; private set; }

        public Trade(SalesPerson person, int quantitySold)
        {
            this.Person = person;
            this.QuantitySold = quantitySold;
        }
    }
}