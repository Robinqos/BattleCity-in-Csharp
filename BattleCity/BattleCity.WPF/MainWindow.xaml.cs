using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using BattleCity.Library.game;

namespace BattleCity.WPF;

public partial class MainWindow : Window
{
    private readonly Hra _hra;
    private readonly DispatcherTimer _gameTimer;
    private DateTime _poslednyTik;

    public MainWindow(string obrazokHraca)
    {
        InitializeComponent();
        this.Loaded += (s, e) => { this.Focus(); };     //ked otvoris, aby ziskalo focus klavesnice (s AI pomocou)

        _hra = new Hra(obrazokHraca, GameCanvas);

        _gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
        _gameTimer.Tick += HernaSlucka;
        _poslednyTik = DateTime.Now;

        _gameTimer.Start();
    }

    private void HernaSlucka(object? sender, EventArgs e)
    {
        var teraz = DateTime.Now;
        var deltaTime = (teraz - _poslednyTik).TotalSeconds;
        _poslednyTik = teraz;

        _hra.Tik(deltaTime);

        if (_hra.JeKoniecHry)
        {
            _gameTimer.Stop();
            var gameOverWindow = new GameOverWindow(_hra.DovodSkoncenia, _hra.Skore, _hra)
            {
                Owner = this
            };
            gameOverWindow.ShowDialog();
        }
    }

    public void RestartGame()
    {
        _gameTimer.Start();
        _poslednyTik = DateTime.Now;
    }

    // Obsluha vstupov
    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.P)
        {
            _hra.PozastavHru();
        }
        else if (e.Key == Key.Space)
        {
            _hra.StlacenyMedzernik();
        }
        else
        {
            _hra.Hrac.PridajStlacenuKlavesu(e.Key);
        }
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {

        _hra.Hrac.OdstranStlacenuKlavesu(e.Key);
    }
}