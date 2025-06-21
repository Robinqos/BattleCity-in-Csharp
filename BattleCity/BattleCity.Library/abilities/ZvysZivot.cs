using System.Windows.Controls;
using BattleCity.Library.tanks;

namespace BattleCity.Library.abilities
{
    class ZvysZivot(int laveX, int horneY, Canvas gameCanvas) : Ability(laveX, horneY, "/BattleCity.Library;component/resources/abilities/tank.png", gameCanvas)
    {
        private readonly string _nazov = "Zvýšený život";

        public override string Nazov() => _nazov;

        public override void VykonajEfekt(Hrac hrac)
        {
            hrac.ZvysHP();
            hrac.ZobrazHP();

            PoVykonani();
        }
    }
}
