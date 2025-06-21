using System.IO;
using System.Windows;
using BattleCity.Library.game;

namespace BattleCity.WPF
{
    public partial class LeaderboardWindow : Window
    {
        public LeaderboardWindow()
        {
            InitializeComponent();
            LoadScores();
        }

        private void LoadScores()
        {
            try
            {
                var scores = new List<(string Name, int Score)>();

                if (File.Exists("leaderboard.csv"))
                {
                    foreach (var line in File.ReadAllLines("leaderboard.csv"))
                    {
                        var parts = line.Split(',');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                        {
                            scores.Add((parts[0], score));
                        }
                    }
                }

                var najlepsiHraci = scores
                    .OrderByDescending(s => s.Score)
                    .Take(10)
                    .ToList();

                var zobrazHracov = new List<Osoba>();
                for (int i = 0; i < najlepsiHraci.Count; i++)
                {
                    zobrazHracov.Add(new Osoba
                    {
                        Poradie = i + 1 + ".",
                        Meno = najlepsiHraci[i].Name,
                        Skore = najlepsiHraci[i].Score
                    });
                }

                ScoreListView.ItemsSource = zobrazHracov;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba pri načítaní skóre: {ex.Message}");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
