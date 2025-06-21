using System.Windows.Controls;
using System.Windows.Threading;
using BattleCity.Library.tanks;

namespace BattleCity.Library.abilities
{
    class Stit(int laveX, int horneY, Canvas gameCanvas) : Ability(laveX, horneY, "/BattleCity.Library;component/resources/abilities/helmet.png", gameCanvas)
    {
        private readonly string _nazov = "Štít";

        public override string Nazov() => _nazov;

        public override void VykonajEfekt(Hrac hrac)
        {
            hrac.ZapniStit();
            PoVykonani();

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(12) };
            timer.Tick += (sender, e) =>
            {
                hrac.VypniStit();
                timer.Stop();
            };
            timer.Start();
        }
    }
}
