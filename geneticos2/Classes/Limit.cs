using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geneticos2.Classes
{
    class Limit
    {
        private int m;
        
        public double A { set; get; }
        public double B { set; get; }
        public int M { 
            set { m = value < 0 ? -value : value; } 
            get => m; 
        }


        // Generar limites
        public static Limit[] generate(Restriction[] restrictions, int n)
        {
            double ax = restrictions[0].Xmin;
            double bx = restrictions[0].Xmax;
            for (int i = 1; i < restrictions.Length; ++i){
                if (ax > restrictions[i].Xmin)
                    ax = restrictions[i].Xmin;
                if (bx < restrictions[i].Xmax)
                    bx = restrictions[i].Xmax;
            }
            double ay = restrictions[0].Ymin;
            double by = restrictions[0].Ymax;
            for (int i = 1; i < restrictions.Length; ++i){
                if (ay > restrictions[i].Ymin)
                    ay = restrictions[i].Ymin;
                if (by < restrictions[i].Ymax)
                    by = restrictions[i].Ymax;
            }

            int mx = Genetics.getMj(ax, bx, n);
            int my = Genetics.getMj(ay, by, n);

            Limit[] limits = {
                new Limit() { A = ax, B = bx, M = mx },
                new Limit() { A = ay, B = by, M = my }
            };

            return limits;
        }
    }
}
