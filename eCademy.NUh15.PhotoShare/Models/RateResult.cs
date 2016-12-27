namespace eCademy.NUh15.PhotoShare.Models
{
    public class RateResult
    {
        public RateResult(double newScore)
        {
            NewScore = newScore;
        }
        public double NewScore { get; set; }
    }
}