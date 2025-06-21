using BattleCity.Library.abilities;
using BattleCity.Library.map;

namespace BattleCity.Library.tanks
{
    static class KontrolaPozicii
    {
        private const int VELKOST_MAPY = 50 * 13;

        public static bool SkontrolujVMape(int mapaLaveX, int mapaHorneY, PoziciaPrePohyblive pozicia)
        {
            if (pozicia.NoveX <= mapaLaveX || pozicia.NoveX + pozicia.Velkost() >= mapaLaveX + VELKOST_MAPY)
            {
                return false;
            }
            return pozicia.NoveY + pozicia.Velkost() < mapaHorneY + VELKOST_MAPY &&
                   pozicia.NoveY > mapaHorneY;
        }

        public static Policko? KontrolaKolizieSPolickom(PoziciaPrePohyblive pozicia, Policko[] policka)
        {
            int noveLaveX = pozicia.NoveX;
            int noveHorneY = pozicia.NoveY;
            int novePraveX = noveLaveX + pozicia.Velkost();
            int noveDolneY = noveHorneY + pozicia.Velkost();

            foreach (var policko in policka)
            {
                if (policko.AktualnyTyp == TypPolicka.ZEM ||
                    policko.AktualnyTyp == TypPolicka.STROM)
                {
                    continue;
                }

                var polickoPoz = policko.Pozicia;
                int polickoLaveX = polickoPoz.LaveX();
                int polickoHorneY = polickoPoz.HorneY();
                int polickoPraveX = polickoPoz.PraveX();
                int polickoDolneY = polickoPoz.DolneY();

                bool kolizia = noveLaveX < polickoPraveX &&
                               novePraveX > polickoLaveX &&
                               noveHorneY < polickoDolneY &&
                               noveDolneY > polickoHorneY;

                if (kolizia)
                {
                    return policko;
                }
            }
            return null;
        }

        public static IPohyblivy? KontrolaStretuPohyblivych(PoziciaPrePohyblive pozicia, List<IPohyblivy> pohyblive)
        {
            int noveLaveX = pozicia.NoveX;
            int noveHorneY = pozicia.NoveY;
            int novePraveX = noveLaveX + pozicia.Velkost();
            int noveDolneY = noveHorneY + pozicia.Velkost();

            foreach (var poh in pohyblive)
            {
                if (poh.Pozicia() == pozicia)
                {
                    continue;
                }

                var pohPoz = poh.Pozicia();
                int inyLaveX = pohPoz.NoveX;
                int inyHorneY = pohPoz.NoveY;
                int inyPraveX = pohPoz.NoveX + pohPoz.Velkost();
                int inyDolneY = pohPoz.NoveY + pohPoz.Velkost();

                bool kolizia = novePraveX >= inyLaveX &&
                               noveLaveX <= inyPraveX &&
                               noveDolneY >= inyHorneY &&
                               noveHorneY <= inyDolneY;

                if (kolizia)
                {
                    return poh;
                }
            }
            return null;
        }

        public static bool KontrolaKolizieSAbilitou(Ability abilita, Hrac hrac)
        {
            var aPoz = abilita.Pozicia();
            var hPoz = hrac.Pozicia();

            return aPoz.LaveX() <= hPoz.PraveX() &&
                   aPoz.PraveX() >= hPoz.LaveX() &&
                   aPoz.HorneY() <= hPoz.DolneY() &&
                   aPoz.DolneY() >= hPoz.HorneY();
        }
    }
}
