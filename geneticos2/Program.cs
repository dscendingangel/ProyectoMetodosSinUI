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

                new Circle { X = 3.3, Y = 11.2, R = 9.7 },
                new Circle { X = 23.9, Y = 8.5, R = 14.17 },
                new Circle { X = 7.3, Y = 18.95, R = 2.58},

            };
            
            double error = 0.22;
            int rounds = 50;
            int size = 10000;
            
            var results = Genetics.calculate(circles, error, n, rounds, size);    

            for (int i = 0; i < results.Length; ++i){
                Console.WriteLine(results[i].Item1);
                Console.WriteLine(results[i].Item2);
                Console.WriteLine(results[i].Item3);
                Console.WriteLine(results[i].Item4);
            }

            sw.Stop();

            Console.WriteLine("Elapsed={0}", sw.Elapsed);

            Console.ReadLine();
        }
    }
}
