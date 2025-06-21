using System.ComponentModel;
using System.IO;
using System.Windows;

namespace BattleCity.WPF
{
    public partial class StartWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _topScoreText = "";
        public string TopScoreText
        {
            get => _topScoreText;
            private set
            {
                _topScoreText = value;
                OnPropertyChanged();
            }
        }

        public StartWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadTopScore();
        }

        private void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ZltyTank_Click(object sender, RoutedEventArgs e)
        {
            SpustHru("/BattleCity.Library;component/resources/tanks/player1.png");
        }

        private void ZelenyTank_Click(object sender, RoutedEventArgs e)
        {
            SpustHru("/BattleCity.Library;component/resources/tanks/player2.png");
        }

        private void SpustHru(string vybranyTank)
        {
            var mainWindow = new MainWindow(vybranyTank);
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Close();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new() { Owner = this };
            helpWindow.ShowDialog();
        }

        private void LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            var leaderboardWindow = new LeaderboardWindow { Owner = this };
            leaderboardWindow.ShowDialog();
        }

        private void LoadTopScore()
        {
            const string filePath = "leaderboard.csv";

            if (!File.Exists(filePath))
            {
                TopScoreText = "Žiadne záznamy";
                return;
            }

            try
            {
                var najlepsiHrac = File.ReadLines(filePath)
                    .Select(line => line.Split(','))
                    .Where(parts => parts.Length == 2)
                    .Select(parts => new
                    {
                        Name = parts[0],
                        Score = int.TryParse(parts[1], out int s) ? s : 0
                    })
                    .OrderByDescending(entry => entry.Score)
                    .FirstOrDefault();

                TopScoreText = (najlepsiHrac != null)
                    ? $"{najlepsiHrac.Name} : {najlepsiHrac.Score}"
                    : "Žiadne záznamy";
            }
            catch
            {
                TopScoreText = "Chyba načítania";
            }
        }
    }
}
