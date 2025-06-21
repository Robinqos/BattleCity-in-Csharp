using System.Windows.Controls;
using System.Windows.Input;
using BattleCity.Library.game;

namespace BattleCity.Library.tanks
{
    public class Hrac(int x, int y, string obrazok, Canvas gameCanvas) : Tank(1, x, y, obrazok, 1, gameCanvas)
    {
        private bool _maStit;
        private readonly List<Obrazok> _hracoveZivoty = [];
        private readonly Canvas _gameCanvas = gameCanvas;
        private readonly HashSet<Key> _stlaceneKlavesy = [];

        public void PridajStlacenuKlavesu(Key klaves)
        {
            _stlaceneKlavesy.Add(klaves);
        }

        public void OdstranStlacenuKlavesu(Key klaves)
        {
            _stlaceneKlavesy.Remove(klaves);
        }

        public void ZnovuZobrazHraca()
        {
            Pozicia().SkryObrazok();
            Pozicia().ZobrazObrazok();
        }

        public void ZobrazHP()
        {
            foreach (var image in _hracoveZivoty)
            {
                _gameCanvas.Children.Remove(image.GetObrazok());
            }
            _hracoveZivoty.Clear();

            int startX = 838;
            for (int i = 0; i < HP; i++)
            {
                var srdco = new Obrazok("/BattleCity.Library;component/resources/icons/heart.png", startX + i * 30, 60, 30);
                _gameCanvas.Children.Add(srdco.GetObrazok());
                _hracoveZivoty.Add(srdco);
            }
        }
        public void ZvysHP()
        {
            if (HP < 3)
            {
                HP++;
            }
        }

        public override bool Zasiahni()
        {
            if (!_maStit)
            {
                bool prezil = base.Zasiahni();
                ZobrazHP();
                return prezil;
            }
            return JeZivy();
        }

        public void ZapniStit() => _maStit = true;
        public void VypniStit() => _maStit = false;

        public override void NovaPozicia()
        {
            const int RYCHLOST = 2;
            int pohybX = 0;
            int pohybY = 0;
            int novaOrientacia = Pozicia().Orientacia;

            bool hore = _stlaceneKlavesy.Contains(Key.W) || _stlaceneKlavesy.Contains(Key.Up);
            bool dole = _stlaceneKlavesy.Contains(Key.S) || _stlaceneKlavesy.Contains(Key.Down);
            bool vlavo = _stlaceneKlavesy.Contains(Key.A) || _stlaceneKlavesy.Contains(Key.Left);
            bool vpravo = _stlaceneKlavesy.Contains(Key.D) || _stlaceneKlavesy.Contains(Key.Right);

            if (hore && !dole && !vlavo && !vpravo)
            {
                pohybY -= RYCHLOST;
                novaOrientacia = (int)Orientacia.Hore;
            }
            if (dole && !hore && !vlavo && !vpravo)
            {
                pohybY += RYCHLOST;
                novaOrientacia = (int)Orientacia.Dole;
            }
            if (vlavo && !dole && !hore && !vpravo)
            {
                pohybX -= RYCHLOST;
                novaOrientacia = (int)Orientacia.Vlavo;
            }
            if (vpravo && !dole && !vlavo && !hore)
            {
                pohybX += RYCHLOST;
                novaOrientacia = (int)Orientacia.Vpravo;
            }

            int novaX = Pozicia().LaveX() + pohybX;
            int novaY = Pozicia().HorneY() + pohybY;

            Pozicia().NovaPozicia(novaX, novaY, novaOrientacia);
        }
    }
}
