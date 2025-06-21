using System.Windows.Controls;

namespace BattleCity.Library.tanks
{
    class Bot(int x, int y, Canvas gameCanvas) : Tank(2, x, y, "/BattleCity.Library;component/resources/tanks/enemyBasic.png", 1, gameCanvas)
    {
        private readonly Random _random = new();
        private bool _mozeSaHybat = true;
        public bool JeZastaveny => _mozeSaHybat;

        public override void NovaPozicia()
        {
            if (!_mozeSaHybat) return;

            const int RYCHLOST = 2;
            int noveX = Pozicia().LaveX();
            int noveY = Pozicia().HorneY();

            switch ((Orientacia)Pozicia().Orientacia)
            {
                case Orientacia.Vlavo:
                    noveX -= RYCHLOST;
                    break;
                case Orientacia.Vpravo:
                    noveX += RYCHLOST;
                    break;
                case Orientacia.Dole:
                    noveY += RYCHLOST;
                    break;
                case Orientacia.Hore:
                    noveY -= RYCHLOST;
                    break;
            }
            Pozicia().NovaPozicia(noveX, noveY, Pozicia().Orientacia);
        }

        public void NastavHybnost(bool hybnost)
        {
            _mozeSaHybat = hybnost;
        }

        public void ZmenOrientaciu()
        {
            int nahoda = _random.Next(1, 5); 
            int novaOrientacia = nahoda * 2;
            Pozicia().NovaPozicia(Pozicia().LaveX(), Pozicia().HorneY(), novaOrientacia);
        }
    }
}