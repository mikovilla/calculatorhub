namespace CH.Web.Models
{
    public partial class RR82018Model
    {
        public class TaxBracket
        {
            public string? Percentage { get; set; }
            public string? Range { get; set; }
            public string? TaxableAmount { get; set; }
        }

        public class PhilhealthBracket
        {
            public int Year { get; set; }
            public decimal PremiumRate { get; set; }
            public decimal MaxContribution { get; set; }
        }

        public class SSSBracket
        {
            public decimal MinSalary { get; set; }
            public decimal MaxSalary { get; set; }
            public decimal EC { get; set; }
            public decimal Contribution { get; set; }
        }

        public class SSSMPFBracket
        {
            public decimal MinSalary { get; set; }
            public decimal MaxSalary { get; set; }
            public decimal MPF { get; set; }
        }

        public class EmploymentShareBracket
        {
            public decimal ERShare { get; set; }
            public decimal EEShare { get; set; }
            public decimal TotalShare { get; set; }
        }
    }
}
