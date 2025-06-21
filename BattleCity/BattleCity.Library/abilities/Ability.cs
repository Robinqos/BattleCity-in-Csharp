using BattleCity.Library.map;
using BattleCity.Library.tanks;
using System.Windows.Controls;

namespace BattleCity.Library.abilities
{
    abstract class Ability
    {
        private readonly Pozicia _pozicia;
        private bool _zobrata;
        public bool BolaZobrata() => _zobrata;
        public Pozicia Pozicia() => _pozicia;

        protected Ability(int laveX, int horneY, string nazov, Canvas gameCanvas)
        {
            const int velkostObrazka = 40;
            _pozicia = new Pozicia(velkostObrazka, laveX, horneY, nazov, gameCanvas);
            _zobrata = false;
        }

        public abstract void VykonajEfekt(Hrac hrac);
        public abstract string Nazov();

        protected void PoVykonani()
        {
            _pozicia.SkryObrazok();
            _zobrata = true;
        }

    }
}
