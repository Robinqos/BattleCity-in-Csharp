using BattleCity.Library.map;
using System.Windows.Controls;
using System.Windows;

namespace BattleCity.Library.tanks
{
    public class Naboj : IPohyblivy
    {
        private readonly PoziciaPrePohyblive _pozicia;
        private readonly bool _dopadol;
        public int Sila { get; }
        public Tank Vlastnik { get; }
        public PoziciaPrePohyblive Pozicia() => _pozicia;

        public Naboj(int laveX, int horneY, int orientacia, int sila, Tank vlastnik)
        {
            var canvas = (Canvas)Application.Current.MainWindow.FindName("GameCanvas");
            _pozicia = new PoziciaPrePohyblive(6, laveX, horneY, "/BattleCity.Library;component/resources/tanks/bullet.png", canvas);
            _pozicia.NovaPozicia(laveX, horneY, orientacia);
            _pozicia.PotvrdNovuPoziciu();

            _dopadol = false;
            Sila = sila;
            Vlastnik = vlastnik;
        }

        public void NovaPozicia()
        {
            const int RYCHLOST = 2;
            if (!_dopadol)
            {
                int posunX = 0;
                int posunY = 0;

                switch ((Orientacia)_pozicia.Orientacia)
                {
                    case Orientacia.Vlavo:
                        posunX -= RYCHLOST;
                        break;
                    case Orientacia.Vpravo:
                        posunX += RYCHLOST;
                        break;
                    case Orientacia.Dole:
                        posunY += RYCHLOST;
                        break;
                    case Orientacia.Hore:
                        posunY -= RYCHLOST;
                        break;
                }

                var noveLaveX = _pozicia.LaveX() + posunX * 3;
                var noveHorneY = _pozicia.HorneY() + posunY * 3;

                _pozicia.NovaPozicia(noveLaveX, noveHorneY, _pozicia.Orientacia);
            }

        }
    }
}
