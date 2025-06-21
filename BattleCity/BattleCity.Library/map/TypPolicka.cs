namespace BattleCity.Library.map
{
    public class TypPolicka
    {
        private TypPolicka(int zasiahnutelnost, string cestaKObrazku)
        {
            Zasiahnutelnost = zasiahnutelnost; 
            CestaKObrazku = cestaKObrazku;
        }

        public int Zasiahnutelnost { get; }
        public string CestaKObrazku { get; }

        //readonly nastavi iba raz, neplni sa tym pamat
        public static readonly TypPolicka STROM = new(0, "/BattleCity.Library;component/resources/map/tree.png");
        public static readonly TypPolicka BETON = new(3, "/BattleCity.Library;component/resources/map/steel.png");
        public static readonly TypPolicka TEHLA = new(1, "/BattleCity.Library;component/resources/map/brick.png");
        public static readonly TypPolicka ZEM = new(0, "/BattleCity.Library;component/resources/map/nothing.png");
        public static readonly TypPolicka VODA = new(0, "/BattleCity.Library;component/resources/map/water.png");
        public static readonly TypPolicka OROL = new(1, "/BattleCity.Library;component/resources/map/eagle.png");

    }
}
