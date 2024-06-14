
using RentalInvestmentAid.Models.Rate;

namespace RentalInvestmentAid.Models.Bank
{
    public class RateInformation
    {
        public int Id { get; set; } 
        public int DurationInYear { get; set; }
        public string Rate { get; set; } = String.Empty;
        public RateType RateType { get; set; }
        public string Title { get; set; } = String.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public override string ToString()
        {
            return $"-RateInformation- Id {Id} DurationInYear : {DurationInYear} Rate : {Rate} Title : {Title} ";
        }

    }
}
