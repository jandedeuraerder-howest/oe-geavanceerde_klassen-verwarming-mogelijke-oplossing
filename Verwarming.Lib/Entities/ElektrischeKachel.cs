using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verwarming.Lib.Entities
{
    public enum ElektrischeKachelTypes { accumulator, straalkachel }

    public class ElektrischeKachel : VerwarmingsToestel
    {
        public ElektrischeKachelTypes ElektrischeKachelType { get; set; }
        public int Verbruik { get; set; }

        public ElektrischeKachel(ElektrischeKachelTypes elektrischeKachelType, int verbruik, string naam, decimal prijs, float opbrengst, int levensduur, int volgNr = 0)
            : base(naam, prijs, opbrengst, levensduur, volgNr)
        {
            ElektrischeKachelType = elektrischeKachelType;
            Verbruik = verbruik;
        }
        public override string ToString()
        {
            return $"{Naam} - {ElektrischeKachelType} - {VolgNr}";

        }

    }
}
