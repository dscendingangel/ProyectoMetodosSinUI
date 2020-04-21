using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geneticos2.Classes
{
    class Genetics
    {
        // Para numeros aleatorios
        static Random random = new Random();

        // Obtiene el numero de bits a utilizar de cierta variable
        public static int getMj(double a, double b, int n)
        {
            double r = (Math.Log(b - a) + Math.Log(10) * n) / Math.Log(2);
            int integerPart = Convert.ToInt32(r);

            if (r - integerPart <= 0)
                return integerPart;

            return integerPart + 1;
        }
        
        // Mapea el valor de una variable de valor máximo de 63 bits
        private static double mapValueFast(char[] chromosome, int begin, int end, double a, double b)
        {
            long val = 0;
            long vmx = 0;
            for (int i = begin; i < end; ++i){
                val = (val << 1) | (long)(chromosome[i] - '0');
                vmx = (vmx << 1) | 1;
            }

            return (long)(b - a) * val / vmx + a;
        }

        // Mapea el valor de una variable de más de 63 bits
        private static double mapValueSlow(char[] chromosome, int begin, int end, double a, double b)
        {
            double val = 0;
            double vmx = 0;
            for (int i = begin; i < end; ++i){
                val = val * 2 + (chromosome[i] - '0');
                vmx = vmx * 2 + 1;
            }

            return (b - a) * val / vmx + a;
        }

        // Obtiene el valor de una variable dado un cromosoma
        // La variable ocupa desde chromosome[begin] a chromosome[end - 1]
        public static double mapValue(char[] chromosome, int begin, int end, double a, double b)
        {
            return end - begin > 63 ? mapValueSlow(chromosome, begin, end, a, b) : mapValueFast(chromosome, begin, end, a, b);
        }

        // Genera una lista de chars de longitud n con numeros aleatorios
        public static char[] randomBits(int n)
        {
            char[] r = new char[n];
            
            for (int i = 0; i < n; ++i)
                r[i] = random.Next(0, 2).ToString()[0];
            
            return r;
        }

        public static double[] getMappedValues(char[] chromosome, Limit[] limits)
        {
            double[] mappedValues = new double[limits.Length];
            for (int i = 0, j = 0; i < mappedValues.Length; ++i){

                mappedValues[i] = mapValue(chromosome, j, j + limits[i].M, limits[i].A, limits[i].B);
                j += limits[i].M;

            }

            return mappedValues;
        }

        public static char[] generateChromosome(Limit[] limits, Restriction[] restrictions)
        {
            char[] chromosome = null;
            bool stay = true;
            
            while (stay) {

                int n = 0;
                for (int i = 0; i < limits.Length; ++i)
                    n += limits[i].M;

                chromosome = randomBits(n);

                // Console.WriteLine(chromosome);

                var mappedValues = getMappedValues(chromosome, limits);

                stay = false;
                for (int i = 0; i < restrictions.Length; ++i){
                    if (!restrictions[i].condition(mappedValues[0], mappedValues[1])){
                        stay = true;
                        break;
                    }
                }

            }

            return chromosome;
        }


        public static char[][] generatePoblation(Limit[] limits, Restriction[] restrictions, int m)
        {
            char[][] poblation = new char[m][];

            for (int i = 0; i < m; ++i)
                poblation[i] = generateChromosome(limits, restrictions);

            return poblation;
        }

    }
}
