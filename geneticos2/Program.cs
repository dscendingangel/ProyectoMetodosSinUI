using geneticos2.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geneticos2
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int n = 1;

            Circle[] circles = {

                new Circle { X = 3.3, Y = 19.3, R = 9.765 },
                new Circle { X = 24, Y = 8.8, R = 14.1702},
                new Circle { X = 7.3, Y = 11.4, R = 2.5851},

            };
            Restriction.initializeZ(circles);

            double relativeError = 0.05;
            double error = Restriction.getAbsoluteError(relativeError, circles.Length);

            Restriction[] restrictions = new Restriction[circles.Length];
            for (int i = 0; i < restrictions.Length; ++i)
                restrictions[i] = new Restriction(circles[i], error);

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

            Console.WriteLine(ax + " " + bx);
            Console.WriteLine(ay + " " + by);

            int mx = Genetics.getMj(ax, bx, n);
            int my = Genetics.getMj(ay, by, n);

            Limit[] limits = {
                new Limit() { A = ax, B = bx, M = mx },
                new Limit() { A = ay, B = by, M = my }
            };
            

            // for (int i = 0; i < 1000; ++i)
            //     Genetics.generateChromosome(limits, restrictions);
            var poblation = Genetics.generatePoblation(limits, restrictions, 100);
            var z = new double[poblation.Length];
            for (int i = 0; i < z.Length; ++i){

                var values = Genetics.getMappedValues(poblation[i], limits);
                z[i] = Restriction.z(values[0], values[1]);
                
            }
                


            sw.Stop();

            Console.WriteLine("Elapsed={0}", sw.Elapsed);

            Console.ReadLine();
        }
    }
}
