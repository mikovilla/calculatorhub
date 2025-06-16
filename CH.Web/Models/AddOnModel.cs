namespace CH.Web.Models
{
    public class AddOnModel
    {
        public int MonthsToPay { get; set; }
        public decimal SimpleInterest { get; set; }
        public decimal FactorRate { get; set; }
        public decimal EffectiveInterestRateA { get; set; }
        public decimal EffectiveInterestRateM { get; set; }
        public decimal MonthlyAmortization { get; set; }
        public decimal Interest { get; set; }
        public decimal TotalPayment { get; set; }
    }
}
