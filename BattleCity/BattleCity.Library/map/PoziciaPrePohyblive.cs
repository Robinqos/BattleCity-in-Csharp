using System.Windows.Controls;

namespace BattleCity.Library.map
{
    public class PoziciaPrePohyblive(int velkost, int laveX, int horneY, string cesta, Canvas parentCanvas) : Pozicia(velkost, laveX, horneY, cesta, parentCanvas)
    {
        private int _noveX;
        private int _noveY;
        private int _novaOrientacia;
        private int _orientacia = 8;
        public int Orientacia => _orientacia;
        public int NoveX => _noveX;
        public int NoveY => _noveY;

        public void NovaPozicia(int x, int y, int novaOrientacia)
        {
            _noveX = x;
            _noveY = y;
            _novaOrientacia = novaOrientacia;
        }


        public void PotvrdNovuPoziciu()
        {
            ZmenPolohu(_noveX, _noveY);


            switch (_novaOrientacia)
            {
                case 4:
                    Obrazok().ZmenUhol(270); // Vlavo
                    break;
                case 6:
                    Obrazok().ZmenUhol(90);  // Vpravo
                    break;
                case 2:
                    Obrazok().ZmenUhol(180); // Dole
                    break;
                default:
                    Obrazok().ZmenUhol(0);   // Hore
                    break;
            }

            _orientacia = _novaOrientacia;
        }

    }
}
