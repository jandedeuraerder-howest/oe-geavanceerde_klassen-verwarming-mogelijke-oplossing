using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Verwarming.Lib;
using Verwarming.Lib.Entities;

namespace H04_Verwarming.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<KostenSimulatie> simulaties = new List<KostenSimulatie>();
        KostenSimulatie huidigeSimulatie;
        VerwarmingsToestel huidigToestel;

        public MainWindow()
        {
            InitializeComponent();
            KostenSimulatie startSimulatie = new KostenSimulatie();
            startSimulatie.LaadVoorbeeld();
            simulaties.Add(startSimulatie);
        }

        private void VerbergGrids()
        {
            grdHaard.Visibility = Visibility.Hidden;
            grdElektrisch.Visibility = Visibility.Hidden;
        }

        void MaakVeldenLeeg(Panel panel)
        {
            foreach (object control in panel.Children)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Clear();
                }
                else if(control is ListBox)
                {
                    ((ListBox)control).SelectedItem = null;
                }
                else if (control is ComboBox)
                {
                    ((ComboBox)control).SelectedItem = null;
                }
                else if(control is CheckBox)
                {
                    ((CheckBox)control).IsChecked = false;
                }
                else if (control is Label)
                {
                    Label label = (Label)control;
                    if (!string.IsNullOrEmpty(label.Name))
                    {
                        label.Content = "";
                    }
                }
            }
        }

        void ToonMelding(string melding, bool isSucces = false)
        {
            tbkFeedback.Visibility = Visibility.Visible;
            tbkFeedback.Text = melding;
            tbkFeedback.Background = isSucces == true ?
                Brushes.Green :
                Brushes.Red;
        }

        void ToonSimulaties()
        {
            lstSimulaties.ItemsSource = simulaties;
            lstSimulaties.Items.Refresh();
        }

        void ToonStatistieken()
        {
            string statistiek = $"Statistiek\n\n";
            statistiek += $"Totaalprijs = {GeefTotaalPrijs().ToString("0.00")}\n";
            double? laagsteverbruik = GeefLaagsteVerbruik();
            if(laagsteverbruik == null)
                statistiek += $"Laagste verbruik is onbekend.\n";
            else
                statistiek += $"Laagste verbruik = {((double)laagsteverbruik).ToString("0.00")}\n";




            tbkStatistieken.Text = statistiek;
        }

        void ToonToestellen()
        {
            lstToestellen.ItemsSource = huidigeSimulatie.Toestellen;
            lstToestellen.Items.Refresh();
        }

        void ToonToestelDetails(VerwarmingsToestel toestel)
        {
            txtNaam.Text = toestel.Naam;
            txtOpbrengst.Text = toestel.OpbrengstPerUurInKw.ToString();
            txtPrijs.Text = toestel.AankoopPrijs.ToString();
            cmbLevensduur.SelectedItem = toestel.LevensduurInJaren;
        }

        private void WisAlleInput()
        {
            MaakVeldenLeeg(grdAlgemeen);
            MaakVeldenLeeg(grdElektrisch);
            MaakVeldenLeeg(grdHaard);
        }

        private void btnSimulatieToevoegen_Click(object sender, RoutedEventArgs e)
        {
            KostenSimulatie simulatie = new KostenSimulatie();
            simulaties.Add(simulatie);
            ToonSimulaties();
        }

        private void lstSimulaties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSimulaties.SelectedItem != null)
            {
                huidigeSimulatie = (KostenSimulatie)lstSimulaties.SelectedItem;
                ToonToestellen();
                ToonStatistieken();
            }
        }

        private void lstToestellen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WisAlleInput();
            VerbergGrids();

            if (lstToestellen.SelectedIndex < 0) return;
            huidigToestel =(VerwarmingsToestel) lstToestellen.SelectedItem;
            txtNaam.Text = huidigToestel.Naam;
            txtPrijs.Text = huidigToestel.AankoopPrijs.ToString("0.00");
            txtOpbrengst.Text = huidigToestel.OpbrengstPerUurInKw.ToString();
            cmbLevensduur.Text = huidigToestel.LevensduurInJaren.ToString();
            //lblRendement.Content = verwarmingstoestel.ren
            if(huidigToestel is Haard)
            {
                grdHaard.Visibility = Visibility.Visible;
                Haard haard = (Haard)huidigToestel;
                if (haard.Afgesloten)
                    chkAfgesloten.IsChecked = true;
                else
                    chkAfgesloten.IsChecked = false;
                lstBrandstof.SelectedItem = haard.Brandstof;
            }
            if(huidigToestel is ElektrischeKachel)
            {
                grdElektrisch.Visibility = Visibility.Visible;
                ElektrischeKachel ekachel = (ElektrischeKachel)huidigToestel;
                txtVerbruik.Text = ekachel.Verbruik.ToString("0.00");
                lstType.SelectedItem = ekachel.ElektrischeKachelType;
            }
            lblRendement.Content = huidigToestel.Rendement.ToString("0.00");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ToonSimulaties();
            VerbergGrids();

            lstSimulaties.SelectedIndex = 0;
            for (int i = 10; i <= 20; i++)
            {
                cmbLevensduur.Items.Add(i);
            }
            //vul lstType
            lstType.Items.Clear();

            lstType.Items.Add(ElektrischeKachelTypes.accumulator);
            lstType.Items.Add(ElektrischeKachelTypes.straalkachel);

            //Vul lstBrandstof
            lstBrandstof.Items.Clear();

            lstBrandstof.Items.Add(Brandstoffen.gas);
            lstBrandstof.Items.Add(Brandstoffen.hout);


            lstToestellen.SelectedIndex = 0;
            grdHaard.Margin = grdElektrisch.Margin;
            tbkFeedback.Visibility = Visibility.Hidden;
        }

        private void btnVoegElektrischToe_Click(object sender, RoutedEventArgs e)
        {
            lstToestellen.SelectedItem = null;
            VerbergGrids();
            grdElektrisch.Visibility = Visibility.Visible;
            txtNaam.Focus();
        }

        private void btnVoegHaardToe_Click(object sender, RoutedEventArgs e)
        {
            lstToestellen.SelectedItem = null;
            VerbergGrids();
            grdHaard.Visibility = Visibility.Visible;
            txtNaam.Focus();
        }

        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string naam = txtNaam.Text;
                decimal prijs = decimal.Parse(txtPrijs.Text) ;
                float opbrengst = float.Parse(txtOpbrengst.Text);
                int levensduur = (int)cmbLevensduur.SelectedItem;

                //Bepaal het volgnr
                int volgnr;
                bool nieuwtoestel = false;
                if (lstToestellen.SelectedIndex < 0)
                {
                    volgnr = 0;
                    nieuwtoestel = true;
                }
                else
                    volgnr = ((VerwarmingsToestel)lstToestellen.SelectedItem).VolgNr;
                //Sla een haard of Elektrische kachel op

                if (grdHaard.Visibility == Visibility.Visible)
                {
                    bool afgesloten = (bool)chkAfgesloten.IsChecked;
                    Brandstoffen brandstof = (Brandstoffen)lstBrandstof.SelectedItem;

                    if (nieuwtoestel)
                    {
                        huidigeSimulatie.Toestellen.Add(new Haard(brandstof, afgesloten, naam, prijs, opbrengst, levensduur, volgnr));
                    }
                    else
                    {
                        Haard haard = (Haard)lstToestellen.SelectedItem;
                        haard.Naam = naam;
                        haard.AankoopPrijs = prijs;
                        haard.OpbrengstPerUurInKw = opbrengst;
                        haard.LevensduurInJaren = levensduur;
                        haard.Brandstof = brandstof;
                        haard.Afgesloten = afgesloten;
                    }
                }
                if(grdElektrisch.Visibility == Visibility.Visible)
                {
                    int verbruik = int.Parse(txtVerbruik.Text);
                    ElektrischeKachelTypes etype = (ElektrischeKachelTypes)lstType.SelectedItem;
                    if(nieuwtoestel)
                    {
                        huidigeSimulatie.Toestellen.Add(new ElektrischeKachel(etype, verbruik, naam, prijs, opbrengst, levensduur, volgnr));
                    }
                    else
                    {
                        ElektrischeKachel ekachel = (ElektrischeKachel)lstToestellen.SelectedItem;
                        ekachel.Naam = naam;
                        ekachel.AankoopPrijs = prijs;
                        ekachel.OpbrengstPerUurInKw = opbrengst;
                        ekachel.LevensduurInJaren = levensduur;
                        ekachel.ElektrischeKachelType = etype;
                        ekachel.Verbruik = verbruik;

                    }

                }
                
            
                ToonToestellen();
                ToonStatistieken();
            }
            catch (Exception fout)
            {
                ToonMelding("Kijk je input na : " + fout.Message);
            }

        }

        private void btnVerwijderToestel_Click(object sender, RoutedEventArgs e)
        {
            if (huidigToestel != null)
            {
                huidigeSimulatie.VerwijderToestel(huidigToestel);
                //Verwijder het huidige toestel
                ToonToestellen();
                ToonStatistieken();
            }
        }
        private double GeefTotaalPrijs()
        {
            double totaalprijs = 0;
            foreach(VerwarmingsToestel toestel in lstToestellen.Items)
            {
                totaalprijs += (double)toestel.AankoopPrijs;
            }
            return totaalprijs;
        }
        private double? GeefLaagsteVerbruik()
        {
            double laagsteverbruik = double.MaxValue;
            foreach (VerwarmingsToestel toestel in lstToestellen.Items)
            {
                if (toestel is ElektrischeKachel)
                {
                    if (((ElektrischeKachel)toestel).Verbruik < laagsteverbruik)
                        laagsteverbruik = ((ElektrischeKachel)toestel).Verbruik;
                }
            }
            if (laagsteverbruik == double.MaxValue)
                return null;
            else
                return laagsteverbruik;
        }


    }
}
