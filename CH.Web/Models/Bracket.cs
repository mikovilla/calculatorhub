namespace CH.Web.Models
{
    public class Bracket
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

        public sealed class TaxCalculator
        {
            decimal monthlySalary;

            public TaxCalculator(decimal monthlyIncome)
            {
                this.monthlySalary = monthlyIncome;
            }

            public (List<TaxBracket>[], string[]) Calculate()
            {
                List<TaxBracket>? pre2023bracket, rr82018bracket;
                string? pre2023Tax, rr82018Tax;
                pre2023bracket = new List<TaxBracket>();
                rr82018bracket = new List<TaxBracket>();
                decimal annualSalary = monthlySalary * 12;
                decimal[] pre2023 = new decimal[] { 0M, .2M, .25M, .3M, .32M, .35M };
                decimal[] rr82018 = new decimal[] { 0M, .15M, .2M, .25M, .3M, .35M };
                Dictionary<decimal, decimal> minMaxSalary = new Dictionary<decimal, decimal>();
                minMaxSalary.Add(0, 250000);
                minMaxSalary.Add(250000.01M, 400000);
                minMaxSalary.Add(400000.01M, 800000);
                minMaxSalary.Add(800000.01M, 2000000);
                minMaxSalary.Add(2000000.01M, 8000000);
                minMaxSalary.Add(8000000.01M, Decimal.MaxValue);

                int taxPercentageElement = 0;
                foreach (KeyValuePair<decimal, decimal> mm in minMaxSalary)
                {
                    if (annualSalary >= mm.Key)
                    {
                        if (annualSalary >= mm.Value)
                        {
                            pre2023bracket.Add(
                                new TaxBracket
                                {
                                    Percentage = $"{(pre2023[taxPercentageElement] * 100).ToString("N2")}%",
                                    Range = $"{mm.Key.ToString("N2")} - {mm.Value.ToString("N2")}",
                                    TaxableAmount = ((mm.Value - mm.Key) * pre2023[taxPercentageElement]).ToString("N2")
                                });
                            rr82018bracket.Add(
                                new TaxBracket
                                {
                                    Percentage = $"{(rr82018[taxPercentageElement] * 100).ToString("N2")}%",
                                    Range = $"{mm.Key.ToString("N2")} - {mm.Value.ToString("N2")}",
                                    TaxableAmount = ((mm.Value - mm.Key) * rr82018[taxPercentageElement]).ToString("N2")
                                });
                        }
                        else
                        {
                            decimal remainder = (annualSalary - mm.Key);
                            if (remainder > 0)
                            {
                                string maxValue = mm.Value >= Decimal.MaxValue ? "above" : mm.Value.ToString("N2");
                                pre2023bracket.Add(
                                    new TaxBracket
                                    {
                                        Percentage = $"{(pre2023[taxPercentageElement] * 100).ToString("N2")}%",
                                        Range = $"{mm.Key.ToString("N2")} - {maxValue}",
                                        TaxableAmount = (remainder * pre2023[taxPercentageElement]).ToString("N2")
                                    });
                                rr82018bracket.Add(
                                    new TaxBracket
                                    {
                                        Percentage = $"{(rr82018[taxPercentageElement] * 100).ToString("N2")}%",
                                        Range = $"{mm.Key.ToString("N2")} - {maxValue}",
                                        TaxableAmount = (remainder * rr82018[taxPercentageElement]).ToString("N2")
                                    });
                            }
                        }
                    }
                    taxPercentageElement++;
                }
                pre2023Tax = (pre2023bracket.Sum(p => Convert.ToDecimal(p.TaxableAmount)) / 12).ToString("N2");
                rr82018Tax = (rr82018bracket.Sum(r => Convert.ToDecimal(r.TaxableAmount)) / 12).ToString("N2");

                return new(new List<TaxBracket>[] { pre2023bracket, rr82018bracket }, new string[] { pre2023Tax, rr82018Tax });
            }
        }

        public sealed class PhilhealthCalculator
        {
            decimal monthlySalary;
            List<PhilhealthBracket> philhealthBracket = new List<PhilhealthBracket>();
            public PhilhealthCalculator(decimal monthlyIncome)
            {
                this.monthlySalary = monthlyIncome;
                this.philhealthBracket = SetupBracket();
            }

            public decimal Calculate(int year)
            {
                if (monthlySalary <= 10000)
                {
                    return 0M;
                }
                var bracket = philhealthBracket.Where(x => x.Year == (year > 2024 ? 2024 : year)).Single();
                var premium = bracket.PremiumRate * monthlySalary;
                return premium > bracket.MaxContribution ? bracket.MaxContribution : premium;
            }

            public Dictionary<PhilhealthBracket, decimal> Calculate()
            {
                Dictionary<PhilhealthBracket, decimal> calculations = new Dictionary<PhilhealthBracket, decimal>();
                foreach (var pb in philhealthBracket)
                {
                    calculations.Add(pb, Calculate(pb.Year));
                }
                return calculations;
            }

            private List<PhilhealthBracket> SetupBracket()
            {
                return new List<PhilhealthBracket>
            {
                new PhilhealthBracket { Year = 2019, PremiumRate = 0.0275M, MaxContribution = 1375 },
                new PhilhealthBracket { Year = 2020, PremiumRate = 0.03M, MaxContribution = 1800 },
                new PhilhealthBracket { Year = 2021, PremiumRate = 0.035M, MaxContribution = 2450 },
                new PhilhealthBracket { Year = 2022, PremiumRate = 0.04M, MaxContribution = 3200 },
                new PhilhealthBracket { Year = 2023, PremiumRate = 0.045M, MaxContribution = 4050 },
                new PhilhealthBracket { Year = 2024, PremiumRate = 0.05M, MaxContribution = 5000 },
            };
            }
        }

        public sealed class PagibigCalculator
        {
            decimal monthlySalary;

            public PagibigCalculator(decimal monthlyIncome)
            {
                this.monthlySalary = monthlyIncome;
            }

            public EmploymentShareBracket Calculate()
            {
                EmploymentShareBracket pb;
                if (monthlySalary < 1000)
                {
                    pb = new EmploymentShareBracket { EEShare = 0M, ERShare = 0M };
                }
                else if (monthlySalary >= 1000 && monthlySalary <= 1500)
                {
                    pb = new EmploymentShareBracket { EEShare = monthlySalary * 0.01M, ERShare = monthlySalary * 0.02M };
                }
                else if (monthlySalary >= 1500 && monthlySalary < 5000)
                {
                    pb = new EmploymentShareBracket { EEShare = monthlySalary * 0.02M, ERShare = monthlySalary * 0.02M };
                }
                else
                {
                    pb = new EmploymentShareBracket { EEShare = monthlySalary * 0.03M, ERShare = monthlySalary * 0.03M };
                }

                pb.ERShare = pb.ERShare > 100 ? 100 : pb.ERShare;
                pb.EEShare = pb.EEShare > 100 ? 100 : pb.EEShare;

                return pb;
            }
        }

        public sealed class SSSCalculator
        {
            decimal monthlySalary;

            public SSSCalculator(decimal monthlyIncome)
            {
                this.monthlySalary = monthlyIncome;
            }

            public List<SSSBracket> ListSSSBracket()
            {
                List<SSSBracket> sssBrackets = new List<SSSBracket>();
                decimal lower = 5250;
                decimal increment = 500;
                decimal contribution = 550;
                int count = 30;
                for (int i = 0; i < count; i++)
                {
                    var bracket = new SSSBracket
                    {
                        MinSalary = lower,
                        MaxSalary = (lower + increment - 0.01m),
                        EC = lower >= 14750 ? 30 : 10,
                        Contribution = contribution
                    };

                    sssBrackets.Add(bracket);
                    lower += increment;
                    contribution += 50;
                }

                return sssBrackets;
            }

            public List<SSSMPFBracket> ListSSSMPFBracket()
            {
                List<SSSMPFBracket> mpfBrackets = new();
                decimal lowerMPF = 20250;
                decimal incrementMPF = 500;
                decimal baseContributionMPF = 50;
                int rows = 30;

                mpfBrackets.Add(new SSSMPFBracket { MinSalary = 0, MaxSalary = 20249.99m, MPF = 0.00m });
                for (int i = 0; i < rows; i++)
                {
                    var bracket = new SSSMPFBracket
                    {
                        MinSalary = lowerMPF,
                        MaxSalary = lowerMPF + incrementMPF - 0.01m,
                        MPF = baseContributionMPF + (i * 50)
                    };
                    mpfBrackets.Add(bracket);
                    lowerMPF += incrementMPF;
                }

                return mpfBrackets;
            }

            public EmploymentShareBracket Calculate()
            {

                var sssBrackets = ListSSSBracket();
                var mpfBrackets = ListSSSMPFBracket();                

                decimal erContribution = ExceedsRange(sssBrackets) ? 2000 : sssBrackets.Single(s => monthlySalary > s.MinSalary && monthlySalary < s.MaxSalary).Contribution;
                decimal ercContribution = ExceedsRange(sssBrackets) ? 30 : sssBrackets.Where(s => monthlySalary > s.MinSalary && monthlySalary < s.MaxSalary).First().EC;
                decimal erMPFContribution = ExceedsRange(sssBrackets) ? 1500 : mpfBrackets.Single(s => monthlySalary > s.MinSalary && monthlySalary < s.MaxSalary).MPF;
                var eeContribution = erContribution / 2;
                var eeMPFContribution = erMPFContribution / 2;
                return new EmploymentShareBracket { 
                    EEShare = eeContribution + eeMPFContribution, 
                    ERShare = erContribution + ercContribution + erMPFContribution, 
                    TotalShare = eeContribution + eeMPFContribution + erContribution + ercContribution + erMPFContribution 
                };
            }

            private bool ExceedsRange(List<SSSBracket> sssBrackets)
            {
                return monthlySalary > sssBrackets.Max(s => s.MaxSalary);
            }
        }

        public sealed class NetCalculator
        {
            public decimal Calculate(decimal monthlySalary, params string[] deductions)
            {
                var ds = deductions.Select(x => Convert.ToDecimal(x)).Sum();
                return monthlySalary - ds;
            }
        }
    }
}
