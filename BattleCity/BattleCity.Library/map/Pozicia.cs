using System.Windows.Controls;
using BattleCity.Library.game;

namespace BattleCity.Library.map
{
    public class Pozicia
    {
        private readonly int velkost;
        private int laveX;
        private int horneY;
        private readonly Obrazok obrazok;
        private readonly Canvas parentCanvas;
        public Obrazok Obrazok() => obrazok;

        public Pozicia(int velkost, int laveX, int horneY, string cesta, Canvas parent)
        {
            this.velkost = velkost;
            this.laveX = laveX;
            this.horneY = horneY;
            parentCanvas = parent;
            obrazok = new Obrazok(cesta, laveX, horneY, this.velkost);
            obrazok.Zobraz(parentCanvas);
        }

        public int Velkost() => velkost;
        public int LaveX() => laveX;
        public int HorneY() => horneY;
        public int DolneY() => horneY + velkost;
        public int PraveX() => laveX + velkost;

        public void ZobrazObrazok() => obrazok.Zobraz(parentCanvas);
        public void SkryObrazok() => obrazok.Skry(parentCanvas);

        public void ZmenObrazok(string cesta) => obrazok.ZmenObrazok(cesta);

        public void ZmenPolohu(int noveX, int noveY)
        {
            laveX = noveX;
            horneY = noveY;
            obrazok.ZmenPolohu(noveX, noveY);
        }

    }
}
