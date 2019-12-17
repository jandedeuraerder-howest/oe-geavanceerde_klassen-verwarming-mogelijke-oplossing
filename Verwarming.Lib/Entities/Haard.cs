using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verwarming.Lib.Entities
{
    public enum Brandstoffen {hout, gas }
    public class Haard : VerwarmingsToestel
    {
        public Brandstoffen Brandstof { get; set; }
        public bool Afgesloten { get; set; } = false;

        public Haard(Brandstoffen brandstof, bool afgesloten, string naam, decimal prijs, float opbrengst, int levensduur, int volgNr = 0 )
            :base (naam,prijs,opbrengst,levensduur,volgNr)
        {
            Brandstof = Brandstof;
            Afgesloten = afgesloten;
        }
        public override string ToString()
        {
            if (Afgesloten)
                return $"{Naam} - {Brandstof} - {VolgNr}";
            else
                return $"Open haard - {VolgNr}";
        }
        public override double Rendement
        {
            get
            {
                double rendement = ((double)AankoopPrijs / (double)LevensduurInJaren) * (double)OpbrengstPerUurInKw;
                if (!Afgesloten)
                    rendement = rendement - rendement * 0.1;
                return rendement;
            }
        }
    }
}
