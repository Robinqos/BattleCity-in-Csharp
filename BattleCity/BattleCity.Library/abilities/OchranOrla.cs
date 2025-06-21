using BattleCity.Library.map;
using BattleCity.Library.tanks;
using System.Windows.Controls;

namespace BattleCity.Library.abilities
{
    class OchranOrla(int laveX, int horneY, Mapa mapa, Canvas gameCanvas) : Ability(laveX, horneY, "/BattleCity.Library;component/resources/abilities/shovel.png", gameCanvas)
    {
        private readonly Mapa _mapa = mapa;
        private readonly string _nazov = "Ochrana orla";

        public override string Nazov() => _nazov;

        public override void VykonajEfekt(Hrac hrac)
        {
            PoVykonani();
            _mapa.NastavOchranuOrla(12000);
        }
    }
}
