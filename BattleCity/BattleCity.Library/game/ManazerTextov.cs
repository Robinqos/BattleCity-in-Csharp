using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace BattleCity.Library.game
{
    class ManazerTextov
    {
        private readonly Canvas _canvas;
        private readonly TextBlock _textSkore;
        private readonly TextBlock _textAbility;
        private readonly TextBlock _textCas;
        private DateTime _casZobrazeniaAbility;
        private readonly TextBlock _textLevel;
        private readonly DispatcherTimer _abilityTimer;
        private readonly Hra _hra;
        private readonly Obrazok _obrazokStavuHry;
        private readonly Obrazok _vlajkaLevelu;

        public ManazerTextov(Canvas canvas, Hra hra)
        {
            _canvas = canvas;
            _hra = hra;

            _obrazokStavuHry = new Obrazok("/BattleCity.Library;component/resources/icons/pause.png", 500, 20, 40);
            _obrazokStavuHry.Zobraz(_canvas);
            _vlajkaLevelu = new Obrazok("/BattleCity.Library;component/resources/icons/flag.png", 130, 340, 60);
            _vlajkaLevelu.Zobraz(_canvas);

            _textLevel = new TextBlock
            {
                Foreground = Brushes.Black,
                FontFamily = new FontFamily("Arial"),
                FontSize = 26,
                FontWeight = FontWeights.Bold
            };

            _textCas = new TextBlock
            {
                Text = "Čas: 0:00",
                Foreground = Brushes.Black,
                FontFamily = new FontFamily("Arial"),
                FontSize = 30,
                FontWeight = FontWeights.Bold,

            };

            _textSkore = new TextBlock
            {
                Text = $"Skóre: {_hra.Skore}",
                Foreground = Brushes.Black,
                FontFamily = new FontFamily("Arial"),
                FontSize = 30,
                FontWeight = FontWeights.Bold
            };

            _textAbility = new TextBlock
            {
                Foreground = Brushes.Yellow,
                FontFamily = new FontFamily("Arial"),
                FontSize = 14,
                FontWeight = FontWeights.Bold
            };
            Canvas.SetLeft(_textAbility, 840);
            Canvas.SetTop(_textAbility, 660);
            _canvas.Children.Add(_textAbility);

            Canvas.SetLeft(_textSkore, 840);
            Canvas.SetTop(_textSkore, 360);
            _canvas.Children.Add(_textSkore);

            Canvas.SetLeft(_textCas, 840);
            Canvas.SetTop(_textCas, 680);
            _canvas.Children.Add(_textCas);

            Canvas.SetLeft(_textLevel, 155);
            Canvas.SetTop(_textLevel, 375);
            _canvas.Children.Add(_textLevel);

            _abilityTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(4) };
            _abilityTimer.Tick += SkryAbilityText;
        }

        public void AktualizujCas(double celkovyCasVsekundach)
        {
            int celkoveSekundy = (int)celkovyCasVsekundach;
            int minuty = celkoveSekundy / 60;
            int sekundy = celkoveSekundy % 60;

            _textCas.Text = $"Čas: {minuty:D2}:{sekundy:D2}";
        }
        public void AktualizujLevel(int level)
        {
            _textLevel.Text = level.ToString();
        }

        public void AktualizujSkore()
        {
            _textSkore.Text = $"Skóre: {_hra.Skore}";
        }

        public void SkryTextSkore()
        {
            if (_textSkore != null)
            {
                _textSkore.Visibility = Visibility.Collapsed;
            }
        }

        public void ZobrazZobratuAbility(string nazovAbility)
        {
            _textAbility.Text = $"Aktivované: {nazovAbility}";
            _textAbility.Visibility = Visibility.Visible;
            _casZobrazeniaAbility = DateTime.Now;
            _abilityTimer.Start();
        }

        private void SkryAbilityText(object? sender, EventArgs e)
        {
            _textAbility.Visibility = Visibility.Collapsed;
            _abilityTimer.Stop();
        }

        public void ZvysSkore(int oKolko)
        {
            _hra.ZvysSkore(oKolko);
            AktualizujSkore();
        }

        public void ZobrazStavHry(bool jePozastavena)
        {
            if (jePozastavena)
            {
                _obrazokStavuHry.ZmenObrazok("/BattleCity.Library;component/resources/icons/play.png");
            }
            else
            {
                _obrazokStavuHry.ZmenObrazok("/BattleCity.Library;component/resources/icons/pause.png");
            }

            _obrazokStavuHry.Zobraz(_canvas);
        }
    }
}
