namespace CustomerOrder.FitNesse.AcceptanceTests
{
    using fit;

    public class Division : ColumnFixture
    {
        public double Numerator { get; set; }
        public double Denominator { get; set; }
            
        public double Quotient()
        {
            return Numerator/Denominator;
        }
    }
}
