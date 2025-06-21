using BattleCity.Library.abilities;
using BattleCity.Library.map;
using BattleCity.Library.tanks;
using System.Windows.Controls;

namespace BattleCity.Library.game
{
    public class Hra
    {
        private readonly Mapa _mapa;
        private readonly Ability?[] _ability = new Ability?[3];
        private readonly List<IPohyblivy> _pohyblive = [];

        private readonly ManazerTextov _manazerTextov;
        private readonly string _obrazokHraca;
        private int _skoreHraca;

        private bool _hraBezi = true;       //na koniec
        private bool _jePozastavena;        //na zastavenie
        private int _aktualnyLevel = 0;
        private double _celkovyCas;
        private string _dovodSkoncenia;
        private DateTime _abilitySpawnPokus = DateTime.Now;

        private Canvas GameCanvas { get; }
        private readonly Random _random = new();
        public Hrac Hrac { get; set; }
        public bool JeKoniecHry => !_hraBezi;
        public string DovodSkoncenia => _dovodSkoncenia;
        public int Skore => _skoreHraca;


        public Hra(string obrazokHraca, Canvas canvas)
        {
            _mapa = new Mapa(canvas);
            GameCanvas = canvas;
            _manazerTextov = new ManazerTextov(canvas, this);
            _dovodSkoncenia = "";
            Hrac = new Hrac(390, 660, obrazokHraca, GameCanvas);
            _obrazokHraca = obrazokHraca;
            Spusti(_aktualnyLevel, obrazokHraca);
        }

        public void Tik(double deltaTime)
        {
            if (!_hraBezi)
            {
                return;
            }
            _manazerTextov.ZobrazStavHry(_jePozastavena);

            if (!_jePozastavena)
            {
                _celkovyCas += deltaTime;
                _manazerTextov.AktualizujCas(_celkovyCas);

                AktualizujPozicie();
                var naVymazanie = new List<IPohyblivy>();
                var zakazPohyb = new List<IPohyblivy>();

                SpracujKolizie(naVymazanie, zakazPohyb);
                OdstranObjekty(naVymazanie);
                PotvrdPozicie(zakazPohyb);
                VytvorAbilitu();
                SkontrolujZberAbility();
                SkontrolujZivotBotov();
                StrelbaBot();
            }
        }

        private void Spusti(int level, string obrazokHraca)
        {
            if (obrazokHraca is not null)
            {
                _mapa.NacitajMapu(level);
                _manazerTextov.AktualizujLevel(level);

                _manazerTextov.AktualizujSkore();

                Hrac.Pozicia().NovaPozicia(390, 660, 8);
                Hrac.Pozicia().PotvrdNovuPoziciu();

                Hrac.ZnovuZobrazHraca();
                Hrac.ZobrazHP();

                _pohyblive.Add(Hrac);
                _pohyblive.Add(new Bot(190, 65, GameCanvas));
                _pohyblive.Add(new Bot(490, 65, GameCanvas));
                _pohyblive.Add(new Bot(790, 65, GameCanvas));
                _mapa.ZnovuZobrazStromy();
            }
        }

        public void StlacenyMedzernik()
        {
            ZaregistrujStrelu(Hrac);
        }

        private void AktualizujPozicie()
        {
            foreach (var pohyblivy in _pohyblive.ToList())
            {
                pohyblivy.NovaPozicia();        //polymorfizmus
            }
        }

        private void SpracujKolizie(List<IPohyblivy> naVymazanie, List<IPohyblivy> zakazPohyb)
        {
            foreach (var objekt in _pohyblive.ToList())
            {
                if (naVymazanie.Contains(objekt))
                {
                    continue;
                }

                if (!KontrolaPozicii.SkontrolujVMape(_mapa.LaveXMapy, _mapa.HorneYMapy, objekt.Pozicia()))
                {
                    SpracujKoliziuSMapou(objekt, zakazPohyb, naVymazanie);
                    continue;
                }

                var koliznyObjekt = KontrolaPozicii.KontrolaStretuPohyblivych(objekt.Pozicia(), _pohyblive);
                if (koliznyObjekt != null)
                {
                    SpracujStretObjektov(objekt, koliznyObjekt, zakazPohyb, naVymazanie);
                }
                else
                {
                    SpracujKoliziuSPolickom(objekt, zakazPohyb, naVymazanie);
                }
            }
        }

        private static void SpracujKoliziuSMapou(IPohyblivy objekt, List<IPohyblivy> zakazPohyb, List<IPohyblivy> naVymazanie)
        {
            if (objekt is Naboj)
            {
                naVymazanie.Add(objekt);
            }
            else if (objekt is Bot bot)
            {
                bot.ZmenOrientaciu();
            }
            else
            {
                zakazPohyb.Add(objekt);
            }
        }

        private void SpracujStretObjektov(IPohyblivy o1, IPohyblivy o2, List<IPohyblivy> zakazPohyb, List<IPohyblivy> naVymazanie)
        {
            if (o1 is Tank t1 && o2 is Tank t2)
            {
                if (t1 is Bot b1 && t2 is Bot b2)
                {
                    b1.ZmenOrientaciu();
                    b2.ZmenOrientaciu();
                }
                else if (t1 is Bot b && t2 is Hrac h)
                {
                    b.ZmenOrientaciu();
                    zakazPohyb.Add(h);
                }
            }
            else if (o1 is Naboj n && o2 is Tank t)
            {
                naVymazanie.AddRange(StretNabojTank(n, t, naVymazanie));
            }
        }

        private List<IPohyblivy> StretNabojTank(Naboj naboj, Tank tank, List<IPohyblivy> naVymazanie)
        {
            if (naboj.Vlastnik is Bot && tank is Bot)
            {
                naVymazanie.Add(naboj);
                return naVymazanie;
            }

            naVymazanie.Add(naboj);


            if (tank is Bot bot)
            {
                bool prezil = bot.Zasiahni();       
                if (!prezil)
                {
                    bot.Pozicia().SkryObrazok();
                    _manazerTextov.ZvysSkore(300);
                    naVymazanie.Add(bot);
                    return naVymazanie;
                }
            }
            if (tank is Hrac)
            {
                if (!tank.Zasiahni()) 
                {
                    _dovodSkoncenia = "Tvoj tank bol zničený!";
                    StopniHru();
                }
            }

            return naVymazanie;
        }

        private void SpracujKoliziuSPolickom(IPohyblivy objekt, List<IPohyblivy> zakazPohyb, List<IPohyblivy> naVymazanie)
        {
            var policko = KontrolaPozicii.KontrolaKolizieSPolickom(objekt.Pozicia(), _mapa.VsetkyPolicka);
            if (policko == null)
            {
                return;
            }

            if (objekt is Hrac hrac)
            {
                zakazPohyb.Add(hrac);
            }
            if (objekt is Bot bot)
            {
                bot.ZmenOrientaciu();
            }
            if (objekt is Naboj naboj)
            {
                int silaNaboja = naboj.Sila;
                int odolnost = policko.AktualnyTyp.Zasiahnutelnost;

                if (odolnost > 0)
                {
                    if (silaNaboja >= odolnost)
                    {
                        if (naboj.Vlastnik is Hrac)
                        {
                            if (policko.AktualnyTyp == TypPolicka.TEHLA)
                            {
                                _manazerTextov.ZvysSkore(10);
                            }
                            else if (policko.AktualnyTyp == TypPolicka.BETON)
                            {
                                _manazerTextov.ZvysSkore(20);
                            }
                        }
                        if (policko.AktualnyTyp == TypPolicka.OROL)
                        {
                            _dovodSkoncenia = "Orol bol zničený!";
                            StopniHru();
                        }
                        policko.NastavTypPolicka(TypPolicka.ZEM);
                    }
                    naVymazanie.Add(naboj);
                }
            }
        }

        public void ZaregistrujStrelu(Tank tank)
        {
            var naboj = tank.Vystrel();
            if (naboj != null)
            {
                _pohyblive.Add(naboj);
            }
            _mapa.ZnovuZobrazStromy();
        }

        private void OdstranObjekty(List<IPohyblivy> naVymazanie)
        {
            foreach (var objekt in naVymazanie)
            {
                if (objekt is Naboj naboj && naboj.Vlastnik != null)
                {
                    naboj.Vlastnik.ResetVystrelov();
                }
                objekt.Pozicia().SkryObrazok();
                _pohyblive.Remove(objekt);
                GameCanvas.Children.Remove(objekt.Pozicia().Obrazok().GetObrazok());
            }
        }

        private void PotvrdPozicie(List<IPohyblivy> zakazPohyb)
        {
            foreach (var objekt in _pohyblive.Where(objekt => !zakazPohyb.Contains(objekt)))
            {
                objekt.Pozicia().PotvrdNovuPoziciu();
            }
        }

        private void SkontrolujZberAbility()
        {
            for (int i = 0; i < _ability.Length; i++)
            {
                var ab = _ability[i];
                if (ab == null || ab.BolaZobrata()) continue;

                foreach (var pohyblivy in _pohyblive.OfType<Hrac>().ToList())
                {
                    if (KontrolaPozicii.KontrolaKolizieSAbilitou(ab, pohyblivy))
                    {
                        _manazerTextov.ZvysSkore(50);  
                        ab.VykonajEfekt(pohyblivy);
                        _manazerTextov.ZobrazZobratuAbility(ab.Nazov());
                        _ability[i] = null;

                        GameCanvas.Children.Remove(ab.Pozicia().Obrazok().GetObrazok());
                    }
                }
            }
        }

        private void VytvorAbilitu()
        {
            if ((DateTime.Now - _abilitySpawnPokus).TotalSeconds < 8)
                return;

            _abilitySpawnPokus = DateTime.Now;

            var volnyIndex = Array.FindIndex(_ability, ab => ab == null || ab.BolaZobrata());
            if (volnyIndex == -1) return;

            if (_random.NextDouble() > 0.5)
            {
                return;
            }

            int typ = _random.Next(5);
            var x = GetRandomPoziciaAbility(190, 790);
            var y = GetRandomPoziciaAbility(60, 665);

            switch (typ)
            {
                case 0:
                    _ability[volnyIndex] = new ZvysZivot(x, y, GameCanvas);
                    break;
                case 1:
                    _ability[volnyIndex] = new VylepsiDelo(x, y, GameCanvas);
                    break;
                case 2:
                    _ability[volnyIndex] = new Stit(x, y, GameCanvas);
                    break;
                case 3:
                    _ability[volnyIndex] = new ZastavCas(x, y, GetBoti(), GameCanvas);
                    break;
                case 4:
                    _ability[volnyIndex] = new OchranOrla(x, y, _mapa, GameCanvas);
                    break;
            }

            var ability = _ability[volnyIndex];
            if (ability != null)                    //aby nebola null referencia
            {
                var pozicia = ability.Pozicia();
                var obrazok = pozicia?.Obrazok()?.GetObrazok();

                if (obrazok != null && !GameCanvas.Children.Contains(obrazok))
                {
                    Canvas.SetLeft(obrazok, x);
                    Canvas.SetTop(obrazok, y);
                    GameCanvas.Children.Add(obrazok);
                }
            }


            _mapa.ZnovuZobrazStromy();
        }

        private int GetRandomPoziciaAbility(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        private List<Bot> GetBoti()
        {
            return [.. _pohyblive.OfType<Bot>()];        //to list
        }

        private void SkontrolujZivotBotov()
        {
            if (!_pohyblive.OfType<Bot>().Any())
            {
                _aktualnyLevel++;
                ZvysLevel(_aktualnyLevel);
            }
        }

        private void ZvysLevel(int novyLevel)
        {
            if (novyLevel > 9)
            {
                _dovodSkoncenia = "Prešiel si celú hru!";
                StopniHru();
                return;
            }

            foreach (var p in _pohyblive.Where(p => p is not tanks.Hrac).ToList())
            {
                p.Pozicia().SkryObrazok();
                GameCanvas.Children.Remove(p.Pozicia().Obrazok().GetObrazok());
            }
            _pohyblive.Clear();
            _mapa.VymazMapu();
            SkryVsetkyAbility();
            Spusti(novyLevel, _obrazokHraca);
        }

        private void SkryVsetkyAbility()
        {
            foreach (var ab in _ability.Where(ab => ab != null))
            {
                ab?.Pozicia()?.SkryObrazok();
            }
            Array.Clear(_ability, 0, _ability.Length);
        }

        private void StrelbaBot()
        {
            foreach (var bot in _pohyblive.OfType<Bot>().Where(b => b.JeZastaveny).ToList())
            {
                ZaregistrujStrelu(bot);
            }
            _mapa.ZnovuZobrazStromy();
        }

        public void StopniHru()
        {
            _hraBezi = false;
        }

        public void RestartujHru()
        {
            StopniHru();
            _pohyblive.Clear();
            _mapa.VymazMapu();
            _skoreHraca = 0;
            _aktualnyLevel = 0;
            _manazerTextov.AktualizujLevel(_aktualnyLevel);
            Hrac.NastavUrovenDela(1, Hrac.PovodnyObrazok);
            Hrac.ResetVystrelov();
            Hrac.HP = 1;
            SkryVsetkyAbility();
            _hraBezi = true;
            Spusti(0, _obrazokHraca);
        }
        public void ZvysSkore(int hodnota)
        {
            _skoreHraca += hodnota;
        }

        public void PozastavHru()
        {
            if (!_hraBezi)
            {
                return;
            }
            _jePozastavena = !_jePozastavena;
        }

    }
}
