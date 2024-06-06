using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models.Gemini
{    public class GeminiPromptData
    {
        public string Informations { get; set; } = String.Empty;
        public string Context { get; set; } = String.Empty;

        public override string ToString()
        {
            return $"Informations : {Informations} Context : {Context}";
        }
    }
}
