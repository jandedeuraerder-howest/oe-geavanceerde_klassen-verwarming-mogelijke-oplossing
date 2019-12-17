using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verwarming.Lib.Entities;

namespace Verwarming.Lib
{
    public class KostenSimulatie
    {
        public List<VerwarmingsToestel> Toestellen { get; set; }

        public string Omschrijving { get; set; }

        public KostenSimulatie(string omschrijving = "")
        {
            Toestellen = new List<VerwarmingsToestel>();
            Omschrijving = string.IsNullOrEmpty(omschrijving.Trim()) ? DateTime.Now.ToString("dd MMM HH:mm") : omschrijving;
        }

        public void LaadVoorbeeld()
        {
            Toestellen.Add(new Haard(Brandstoffen.gas, true, "Gaswonder15000", 8300M, 15000F, 18));
            Toestellen.Add(new Haard(Brandstoffen.gas, true, "Gaswonder12000", 6999.99M, 12000F, 14));
            Toestellen.Add(new Haard(Brandstoffen.hout, false, "Saey basic", 4300M, 9000F, 20));
            Toestellen.Add(new Haard(Brandstoffen.hout, false, "Saey deluxe", 7515M, 14000F, 20));
            Toestellen.Add(new ElektrischeKachel(ElektrischeKachelTypes.straalkachel, 2000, "Krups Heater 2000", 249.99M, 2000F, 10));
            Toestellen.Add(new ElektrischeKachel(ElektrischeKachelTypes.straalkachel, 3000, "Krups Heater 3000", 312M, 3000F, 10));
            Toestellen.Add(new ElektrischeKachel(ElektrischeKachelTypes.accumulator, 3000, "Krups Accu 3000", 699.99M, 3000F, 20));


        }
        public bool ZitInList(VerwarmingsToestel zoekToestel)
        {
            bool gevonden = false;
            foreach(VerwarmingsToestel toestel in Toestellen)
            {
                if(toestel.VolgNr == zoekToestel.VolgNr && toestel != zoekToestel)
                {
                    gevonden = true;
                    break;
                }
            }
            return gevonden;
        }
        public void VerwijderToestel(VerwarmingsToestel wisToestel)
        {
            Toestellen.Remove(wisToestel);
        }
        public override string ToString()
        {
            return Omschrijving;
        }

    }
}
