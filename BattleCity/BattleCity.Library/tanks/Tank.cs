using System.Windows.Controls;
using BattleCity.Library.map;

namespace BattleCity.Library.tanks
{
    public abstract class Tank : IPohyblivy
    {
        private int _hp;
        private int _vystreleneNaboje;
        private int _urovenDela;
        private string _nazovAktualnehoObrazku;
        private readonly string _nazovPovodnehoObrazku;
        private readonly PoziciaPrePohyblive _pozicia;
        public string AktualnyObrazok => _nazovAktualnehoObrazku;
        public string PovodnyObrazok => _nazovPovodnehoObrazku;
        public abstract void NovaPozicia();
        public PoziciaPrePohyblive Pozicia() => _pozicia;

        public void ResetVystrelov() => _vystreleneNaboje = Math.Max(0, _vystreleneNaboje - 1);

        public Tank(int hp, int x, int y, string nazovObrazku, int urovenDela, Canvas gameCanvas)
        {
            _hp = hp;
            const int velkostTanku = 40;
            _pozicia = new PoziciaPrePohyblive(velkostTanku, x, y, nazovObrazku, gameCanvas);
            _urovenDela = urovenDela;
            _nazovAktualnehoObrazku = nazovObrazku;
            _nazovPovodnehoObrazku = nazovObrazku;
        }

        public Naboj? Vystrel()
        {
            int maxNaboje = _urovenDela >= 2 ? 2 : 1;
            if (_vystreleneNaboje >= maxNaboje)
            {
                return null;
            }

            _vystreleneNaboje++;
            const int korekcia = 3;

            return (Orientacia)_pozicia.Orientacia switch
            {
                Orientacia.Vlavo => new Naboj(
                                        _pozicia.LaveX() - korekcia * 3,
                                        _pozicia.HorneY() + _pozicia.Velkost() / 2 - korekcia,
                                        (int)Orientacia.Vlavo,
                                        _urovenDela,
                                        this),
                Orientacia.Vpravo => new Naboj(
                                        _pozicia.PraveX() + korekcia * 3,
                                        _pozicia.HorneY() + _pozicia.Velkost() / 2 - korekcia,
                                        (int)Orientacia.Vpravo,
                                        _urovenDela,
                                        this),
                Orientacia.Dole => new Naboj(
                                        _pozicia.LaveX() - korekcia + _pozicia.Velkost() / 2,
                                        _pozicia.DolneY() + korekcia * 3,
                                        (int)Orientacia.Dole,
                                        _urovenDela,
                                        this),
                _ => new Naboj(
                                        _pozicia.LaveX() - korekcia + _pozicia.Velkost() / 2,
                                        _pozicia.HorneY() - korekcia * 3,
                                        (int)Orientacia.Hore,
                                        _urovenDela,
                                        this),
            };
        }

        public int HP
        {
            get => _hp;
            set => _hp = Math.Clamp(value, 0, 3); // clamp - od 0 do 3
        }

        public virtual bool Zasiahni()
        {
            _hp--;
            return _hp > 0;
        }

        public bool JeZivy()
        {
            return _hp > 0;
        }

        public int UrovenDela
        {
            get => _urovenDela;
            set => _urovenDela = value;
        }

        public void NastavUrovenDela(int uroven, string obrazok)
        {
            _nazovAktualnehoObrazku = obrazok;
            _pozicia.ZmenObrazok(obrazok);
            UrovenDela = uroven;
        }

    }
}
