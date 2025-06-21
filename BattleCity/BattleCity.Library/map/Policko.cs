using System.Windows.Controls;

namespace BattleCity.Library.map
{
    public class Policko
    {
        private readonly Pozicia pozicia;
        private TypPolicka typPolicka;
        public Pozicia Pozicia => pozicia;
        public TypPolicka AktualnyTyp => typPolicka;

        public Policko(TypPolicka typPolicka, int x, int y, Canvas parentCanvas)
        {
            const int VELKOST = 50;
            pozicia = new Pozicia(VELKOST, x, y, typPolicka.CestaKObrazku, parentCanvas);
            this.typPolicka = typPolicka;
        }

        public void NastavTypPolicka(TypPolicka novyTyp)
        {
            typPolicka = novyTyp;
            pozicia.ZmenObrazok(novyTyp.CestaKObrazku);
        }

    }
}
