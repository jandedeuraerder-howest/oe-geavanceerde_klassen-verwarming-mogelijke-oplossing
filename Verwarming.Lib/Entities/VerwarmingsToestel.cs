using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verwarming.Lib.Entities
{
    public class VerwarmingsToestel
    {
        private static int AantalInstances=0;
        private int volgnr;
        public int VolgNr { get { return volgnr; } }

        public string Naam { get; set; }

        public decimal AankoopPrijs { get; set; }

        public float OpbrengstPerUurInKw { get; set; }

        public int LevensduurInJaren { get; set; } = 10;

        public VerwarmingsToestel(string naam, decimal prijs, float opbrengst, int levensduur, int volgNr = 0)
        {
            Naam = naam;
            AankoopPrijs = prijs;
            OpbrengstPerUurInKw = opbrengst;
            LevensduurInJaren = levensduur;
            AantalInstances++;
            if( volgNr == 0)
                volgNr = AantalInstances;
            this.volgnr = volgNr;
        }
        public virtual double Rendement
        {
            get
            {
                return ((double)AankoopPrijs / (double)LevensduurInJaren) * (double)OpbrengstPerUurInKw;

            }
        }
    }
}
