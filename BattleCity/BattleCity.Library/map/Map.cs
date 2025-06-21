using System.IO;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BattleCity.Library.map
{
    public class Mapa
    {
        private const int SIRKA_HRACEJ_PLOCHY = 1024;
        private const int VYSKA_HRACEJ_PLOCHY = 768;
        private const int ROZMER_POLICKA = 50;
        private const int POCET_POLICOK = 13;

        private readonly int _rozmerHracejPlochy = POCET_POLICOK * ROZMER_POLICKA;
        private readonly int _zaciatokMapyX;
        private readonly int _zaciatokMapyY;
        private readonly Policko[] _policka;
        private readonly Canvas _parentCanvas;
        public int LaveXMapy => _zaciatokMapyX;
        public int HorneYMapy => _zaciatokMapyY;
        public Policko[] VsetkyPolicka => _policka;

        public Mapa(Canvas parentCanvas)
        {
            _parentCanvas = parentCanvas;
            _zaciatokMapyX = (SIRKA_HRACEJ_PLOCHY - _rozmerHracejPlochy) / 2;
            _zaciatokMapyY = (VYSKA_HRACEJ_PLOCHY - _rozmerHracejPlochy) / 2;
            _policka = new Policko[POCET_POLICOK * POCET_POLICOK];
        }

        public void NacitajMapu(int cisloMapy)
        {
            var cesta = Path.Combine(Directory.GetCurrentDirectory(), $"resources/levels/{cisloMapy}.txt");

            if (!File.Exists(cesta))
                throw new FileNotFoundException("Subor mapy sa nenasiel!", cesta);

            StreamReader reader = new(cesta);
            try
            {
                reader = new StreamReader(cesta);
                int pocitadlo = 0;
                int riadok = 0;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine()?.ToCharArray();
                    if (line == null) continue;

                    int stlpec = 0;
                    foreach (var znak in line)
                    {
                        if (char.IsWhiteSpace(znak))
                        {
                            continue;
                        }
                        if (stlpec >= POCET_POLICOK)
                        {
                            break;
                        }

                        var x = _zaciatokMapyX + stlpec * ROZMER_POLICKA;
                        var y = _zaciatokMapyY + riadok * ROZMER_POLICKA;

                        _policka[pocitadlo] = znak switch
                        {
                            'T' => new Policko(TypPolicka.TEHLA, x, y, _parentCanvas),
                            'B' => new Policko(TypPolicka.BETON, x, y, _parentCanvas),
                            'V' => new Policko(TypPolicka.VODA, x, y, _parentCanvas),
                            'O' => new Policko(TypPolicka.OROL, x, y, _parentCanvas),
                            'S' => new Policko(TypPolicka.STROM, x, y, _parentCanvas),
                            _ => new Policko(TypPolicka.ZEM, x, y, _parentCanvas)
                        };

                        pocitadlo++;
                        stlpec++;
                    }
                    riadok++;
                }
            }
            finally
            {
                reader?.Dispose();
            }
        }

        public void ZnovuZobrazStromy()
        {
            foreach (var policko in _policka)
            {
                if (policko?.AktualnyTyp == TypPolicka.STROM)
                {
                    policko.Pozicia.SkryObrazok();
                    policko.Pozicia.ZobrazObrazok();
                }
            }
        }

        public void NastavOchranuOrla(int dobaOchranyMs)
        {
            var zmeneneIndexy = new List<int>();
            var povodneTypy = new List<TypPolicka>();

            for (var i = 0; i < _policka.Length; i++)
            {
                if (_policka[i]?.AktualnyTyp != TypPolicka.OROL) continue;

                var riadok = i / POCET_POLICOK;
                var stlpec = i % POCET_POLICOK;

                for (var deltaR = -1; deltaR <= 1; deltaR++)
                {
                    for (var deltaS = -1; deltaS <= 1; deltaS++)
                    {
                        var novaRiadok = riadok + deltaR;
                        var novyStlpec = stlpec + deltaS;

                        if (novaRiadok < 0 || novaRiadok >= POCET_POLICOK ||
                            novyStlpec < 0 || novyStlpec >= POCET_POLICOK)
                        {
                            continue;
                        }

                        var index = novaRiadok * POCET_POLICOK + novyStlpec;
                        if (_policka[index]?.AktualnyTyp == TypPolicka.OROL)
                        {
                            continue;
                        }

                        if (zmeneneIndexy.Contains(index))
                        {
                            continue;
                        }

                        zmeneneIndexy.Add(index);
                        povodneTypy.Add(_policka[index].AktualnyTyp);
                        _policka[index].NastavTypPolicka(TypPolicka.BETON);
                    }
                }
            }

            //s AI pomocou
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(dobaOchranyMs) };
            timer.Tick += (sender, args) =>
            {
                for (var i = 0; i < zmeneneIndexy.Count; i++)
                {
                    var index = zmeneneIndexy[i];
                    _policka[index].NastavTypPolicka(povodneTypy[i]);
                }
                timer.Stop();
            };
            timer.Start();
        }

        public void VymazMapu()
        {
            foreach (var policko in _policka)
            {
                policko?.Pozicia.SkryObrazok();
            }
        }
    }
}
