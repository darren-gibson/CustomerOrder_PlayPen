namespace CustomerOrder.AcceptanceTests.Helpers
{
    using System;
    using Model;
    using PriceServiceStub;
    using TechTalk.SpecFlow;

    class PriceSetup
    {
        private SimplePriceGateway _simplePriceGateway;

        public PriceSetup()
        {
            _simplePriceGateway = Application.Registry.Resolve<SimplePriceGateway>();
        }

        public void Setup(string productId, Money unitPrice, string unitOfMeasure)
        {
            var quantity = ConvertToQuantity(1, unitOfMeasure);
            _simplePriceGateway.SetPrice(productId, new QuantityPrice(unitPrice, quantity));
        }

        private Quantity ConvertToQuantity(decimal amount, string unitOfMeasure)
        {
            var uom = (UnitOfMeasure)Enum.Parse(typeof(UnitOfMeasure), unitOfMeasure);
            return new Quantity(amount, uom);
        }

        public void Setup(Table table)
        {
            foreach (var tableRow in table.Rows)
            {
                var productId = tableRow["Product Id"];
                var unitPrice = ConvertToMoney(tableRow["Unit Price"]);
                var sellByUnitOfMeasure = tableRow["Sell by UOM"];

                Setup(productId, unitPrice, sellByUnitOfMeasure);
            }
        }

        private static Money ConvertToMoney(string moneyString)
        {
            var moneyStrings = moneyString.Split(new[] { ' ' });
            var amount = decimal.Parse(moneyStrings[0]);
            var currencyCode = (Currency)Enum.Parse(typeof(Currency), moneyStrings[1]);
            return new Money(currencyCode, amount);
        }
    }
}
