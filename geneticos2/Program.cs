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
            
            double error = 0.05;

            int rounds = 10;
            
            Genetics.calculate(circles, error, n);    


            sw.Stop();

            Console.WriteLine("Elapsed={0}", sw.Elapsed);



            Console.ReadLine();
        }
    }
}
