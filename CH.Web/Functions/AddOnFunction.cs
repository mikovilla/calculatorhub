namespace CH.Web.Functions
{
    public class AddOnFunction
    {
        /// <summary>
        /// Finds the monthly effective interest rate using a binary search.
        /// </summary>
        /// <param name="principal">The principal loan amount.</param>
        /// <param name="payment">The fixed monthly payment.</param>
        /// <param name="n">Number of months in the loan term.</param>
        /// <returns>The monthly effective interest rate (as a decimal; e.g., 0.007 for 0.7%).</returns>
        public static double CalculateMonthlyRate(double principal, double payment, int n)
        {
            // Define lower and upper boundaries for the rate.
            double low = 0.0;
            double high = 1.0; // Starting with 100% monthly rate as an upper bound.
            double mid = 0.0;

            // Iterate until convergence (e.g., 100 iterations will be enough for practical purposes)
            for (int i = 0; i < 100; i++)
            {
                mid = (low + high) / 2;
                // Calculate the present value of the annuity using the candidate rate mid.
                double presentValue = payment * (1 - Math.Pow(1 + mid, -n)) / mid;

                if (presentValue > principal)
                {
                    // The rate is too low because it gives a higher annuity factor.
                    low = mid;
                }
                else
                {
                    // The rate might be too high.
                    high = mid;
                }
            }
            return mid;
        }
    }
}
