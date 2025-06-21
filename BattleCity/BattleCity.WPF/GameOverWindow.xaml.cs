using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BattleCity.Library.game;

namespace BattleCity.WPF
{
    public partial class GameOverWindow : Window
    {
        private readonly int _skore;
        private readonly Hra _hra;
        private bool _jePredvolenyText = true;
        private bool _skoreUlozene = false;
        public string SkoreText { get; }

        public GameOverWindow(string dovod, int skore, Hra hra)
        {
            InitializeComponent();

            _skore = skore;
            _hra = hra;

            DovodText.Text = dovod;
            DovodText.Foreground = dovod.Contains("Prešiel si celú hru")
                ? Brushes.Gold
                : Brushes.Red;

            SkoreText = $"Tvoje skóre: {skore}";
            DataContext = this;

            MenoHraca.Foreground = Brushes.Gray;
        }


        private void MenoHraca_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_jePredvolenyText)
            {
                MenoHraca.Text = "";
                MenoHraca.Foreground = Brushes.Black;
                _jePredvolenyText = false;
            }
        }

        private void MenoHraca_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MenoHraca.Text))  //ak nic, vrati spat Zadaj svoje meno
            {
                MenoHraca.Text = "Zadaj svoje meno";
                MenoHraca.Foreground = Brushes.Gray;
                _jePredvolenyText = true;
            }
        }

        private void UlozSkore_Click(object sender, RoutedEventArgs e)
        {
            if (_skoreUlozene)
            {
                MessageBox.Show("Skóre bolo už uložené!", "Informácia", MessageBoxButton.OK);
                return;
            }
            if (_jePredvolenyText || string.IsNullOrWhiteSpace(MenoHraca.Text.Trim()))
            {
                MessageBox.Show("Zadajte svoje meno pre uloženie skóre!",
                                "Chýbajúce meno",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            try
            {
                var zaznam = $"{MenoHraca.Text.Trim()},{_skore}";
                File.AppendAllText("leaderboard.csv", $"{zaznam}\n");

                _skoreUlozene = true;

                // zatmavenie 
                MenoHraca.IsEnabled = false;
                UlozSkoreButton.IsEnabled = false;
                UlozSkoreButton.Content = "Skóre uložené";
                UlozSkoreButton.Foreground = Brushes.Gray;

                UlozSkoreButton.Cursor = Cursors.No;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba pri ukladaní: {ex.Message}", "Chyba", MessageBoxButton.OK);
            }
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            var startWindow = new StartWindow();
            startWindow.Show();

            Owner?.Close();     //ak nieje null, tak close
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
