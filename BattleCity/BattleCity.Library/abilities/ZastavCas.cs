using System.Windows.Controls;
using System.Windows.Threading;
using BattleCity.Library.tanks;

namespace BattleCity.Library.abilities
{
    class ZastavCas(int laveX, int horneY, IEnumerable<Bot> boti, Canvas gameCanvas) : Ability(laveX, horneY, "/BattleCity.Library;component/resources/abilities/timer.png", gameCanvas)
    {
        private readonly IEnumerable<Bot> _botiNaMape = [.. boti];      //to list
        private readonly string _nazov = "Zastavený čas";

        public override string Nazov() => _nazov;

        public override void VykonajEfekt(Hrac hrac)
        {
            foreach (var bot in _botiNaMape)
            {
                bot.NastavHybnost(false);
            }

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(8) };         // s AI
            timer.Tick += (sender, e) =>
            {
                foreach (var bot in _botiNaMape)
                {
                    bot.NastavHybnost(true);
                }
                timer.Stop(); 
            };
            timer.Start();

            PoVykonani();
        }
    }
}
