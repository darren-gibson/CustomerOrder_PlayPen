namespace CustomerOrder.AcceptanceTests.Contract
{
    using System;
    using Newtonsoft.Json;

    internal class Quantity
    {
        // ReSharper disable once InconsistentNaming - I like this one :)
        [JsonProperty(PropertyName = "uom")]
        public string UOM { get; set; }
        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }

        public static Quantity Parse(string quantityString)
        {
            try
            {
                var parts = quantityString.Split(' ');
                return new Quantity
                {
                    Amount = decimal.Parse(parts[0]),
                    UOM = parts[1]
                };
            }
            catch (Exception e)
            {
                throw new Exception(
                    string.Format("Expected Quantity format is invalid, expected 'Amount UOM' e.g. '1 Each' - actual was '{0}'",
                        quantityString), e);
            }

        }
    }
}