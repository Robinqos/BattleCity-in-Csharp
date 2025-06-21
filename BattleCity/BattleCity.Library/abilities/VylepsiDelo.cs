using System.Windows.Controls;
using BattleCity.Library.tanks;

namespace BattleCity.Library.abilities
{
    class VylepsiDelo(int laveX, int horneY, Canvas gameCanvas) : Ability(laveX, horneY, "/BattleCity.Library;component/resources/abilities/star.png", gameCanvas)
    {
        private readonly string _nazov = "Vylepšené delo";

        public override string Nazov() => _nazov;

        //S AI pomocou
        public override void VykonajEfekt(Hrac hrac)
        {
            int aktualna = hrac.UrovenDela;
            const int MAX_UROVEN = 3;

            if (aktualna < MAX_UROVEN)
            {
                int novaUroven = aktualna + 1;
                string povodnyNazov = hrac.AktualnyObrazok.Replace(".png", "");

                char typHraca = povodnyNazov[^1];  //posledny znak

                string zakladTypu = povodnyNazov[..^1]; //bez posledneho

                string novyNazov = $"{zakladTypu}{typHraca}{novaUroven}.png";
                hrac.NastavUrovenDela(novaUroven, novyNazov);
            }
            PoVykonani();
        }
    }
}
