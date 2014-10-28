namespace CustomerOrder.Model
{
    using System;

    /// <summary>
    /// Represents world currency by numeric and alphabetic values, as per ISO 4217:
    /// http://www.iso.org/iso/currency_codes_list-1.
    /// </summary>
    [Serializable]
    public enum Currency : ushort
    {
        USD = 840,
        CAD = 124,
        EUR = 978,
        AUD = 036,
        GBP = 826,
        INR = 356,
        JPY = 392,
        CHF = 756,
        NZD = 554
    }
}
