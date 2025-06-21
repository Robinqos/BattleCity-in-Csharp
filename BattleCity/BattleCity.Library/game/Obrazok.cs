using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BattleCity.Library.game
{   
    //Pomocou AI vacsina triedy
    public class Obrazok
    {
        private readonly Image obrazok;
        private double uhol;

        public Obrazok(string cesta, int x, int y, int velkost)
        {
            obrazok = new Image
            {
                Width = velkost,
                Height = velkost
            };
            ZmenObrazok(cesta);
            ZmenPolohu(x, y);
            uhol = 0;
        }

        public Image GetObrazok() => obrazok;

        public void Zobraz(Canvas parent)
        {
            if (!parent.Children.Contains(obrazok))
                parent.Children.Add(obrazok);
        }

        public void Skry(Canvas parent)
        {
            if (parent.Children.Contains(obrazok))
                parent.Children.Remove(obrazok);
        }

        public void ZmenObrazok(string cesta)
        {
            var bitmap = new BitmapImage(new Uri(cesta, UriKind.RelativeOrAbsolute));
            obrazok.Source = bitmap;
        }

        public void ZmenPolohu(int x, int y)
        {
            Canvas.SetLeft(obrazok, x);         //horizont x
            Canvas.SetTop(obrazok, y);          //vertikal y
        }

        public void ZmenUhol(double uhol)
        {
            this.uhol = uhol;
            var otocenie = new RotateTransform(this.uhol, obrazok.Width / 2, obrazok.Height / 2);
            obrazok.RenderTransform = otocenie;
        }
    }
}
