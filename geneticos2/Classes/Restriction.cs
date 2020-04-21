using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geneticos2.Classes
{
    class Restriction
    {
        public Func<double, double, bool> condition;
        public double Xmax { get; }
        public double Ymax { get; }
        public double Xmin { get; }
        public double Ymin { get; }

        public Restriction(Circle circle, double error)
        {
            condition = createCircleRestriction(circle, error);

            Xmax = Math.Sqrt(error + square(circle.R)) + circle.X;
            Ymax = Math.Sqrt(error + square(circle.R)) + circle.Y;

            Xmin = -Math.Sqrt(error + square(circle.R)) + circle.X;
            Ymin = -Math.Sqrt(error + square(circle.R)) + circle.Y;
        }

        // Cosas estaticas
        public static Func<double, double, double> z = null;

        public static void initializeZ(Circle[] circles)
        {
            z = (double x, double y) => {
                double result = 0;

                Circle[] c = new Circle[circles.Length];
                Array.Copy(circles, c, circles.Length);

                for (int i = 0; i < c.Length; ++i)
                    result += square(square(x - c[i].X) + square(y - c[i].Y) - square(c[i].R));

                return result;
            };
        }

        // Cuadrado simplificado
        static double square(double a) => a * a;
        // Crea una nueva funcion con los parametros dados
        static Func<double, double, bool> createCircleRestriction(Circle circle, double error)
        {
            return (double x, double y) => {
                // Console.WriteLine(x + " " + y + " - " + (square(x - circle.X) + square(y - circle.Y) - square(circle.R)) + " e: " + error);
                return square(x - circle.X) + square(y - circle.Y) - square(circle.R) <= error;
            };
        }

        public static double getAbsoluteError(double relativeError, int numOfRestrictions) => Math.Sqrt(z(0, 0)) * relativeError / numOfRestrictions;
    }
}
