﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geneticos2.Classes
{
    class Genetics
    {
        //Definimos el límite de tiempo
        static int limitOfExecution = 50000000, err;
        // Para numeros aleatorios
        static Random random = new Random();

        // Obtiene el numero de bits a utilizar de cierta variable
        //Recibe limite superior e inferior y los bits de la variable, y los bits de presición
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
        //Obtiene los valorea de n variables dado el cromosoma y los limites correspodientes
        public static double[] getMappedValues(char[] chromosome, Limit[] limits)
        {
            double[] mappedValues = new double[limits.Length];
            for (int i = 0, j = 0; i < mappedValues.Length; ++i){

                mappedValues[i] = mapValue(chromosome, j, j + limits[i].M, limits[i].A, limits[i].B);
                j += limits[i].M;

            }

            return mappedValues;
        }

        // Evaluamos si un hijo es valido o no respceto a las restricciones
        static bool evaluate(char[] chromosome, Limit[] limits, Restriction[] restrictions)
        {
            var mappedValues = getMappedValues(chromosome, limits);

            for (int i = 0; i < restrictions.Length; ++i)
                if (!restrictions[i].condition(mappedValues[0], mappedValues[1]))
                    return false;

            return true;
            
        }
        
        //Genera un cromosama valido, generando cromosomas al azar y comprobando que sean validos con evaluate
        // hasta tener uno válido, devuelve este
        public static char[] generateChromosome(Limit[] limits, Restriction[] restrictions)
        {
            char[] chromosome = null;
            bool stay = true;
            
            while (stay && err < limitOfExecution)
            {
                err++
                int n = 0;
                for (int i = 0; i < limits.Length; ++i)
                    n += limits[i].M;

                chromosome = randomBits(n);

                stay = !evaluate(chromosome, limits, restrictions);

            }

            if (err == limitOfExecution)
                throw new StackOverflowException("Limite de iteraciones alcanzado.");

            return chromosome;
        }

        // Genera población aleatoria válida de m longitud
        public static char[][] generatePoblation(Limit[] limits, Restriction[] restrictions, int m)
        {
            char[][] poblation = new char[m][];

            for (int i = 0; i < m; ++i)
                poblation[i] = generateChromosome(limits, restrictions);

            return poblation;
        }
        
        //Función para hacer mutación a un cromosoma, se elige uno de los bits al azar y se altera este
        public static char[] mutation(char[] chromosome)
        {
            int index = random.Next(0, chromosome.Length);
            if (chromosome[index] == '1')
            {
                chromosome[index] = '0';
                return chromosome;
            }
            chromosome[index] = '1';

            return chromosome;
        }
        
        //Cruza de cromosomas, recibe los padres y retorna al la cruza de estos, eligiendo tambien la posición al azar
        public static char[] crossover(char[] parent1, char[] parent2)
        {

            int chromosome_size = parent1.Length;

            char[] child = new char[chromosome_size];

            int n = random.Next(0, chromosome_size);

            for (int i = 0; i < n; ++i)
                child[i] = parent1[i]; 

            for (int i = n; i < chromosome_size; ++i)
                child[i] = parent2[i];

            return child;


        }

        // Metodo maestro, integra todo
        public static (int, double, double, double)[] calculate(Circle[] circles, double error, int n, int rounds, int size)
        {
            err = 0;

            var answer = new (int, double, double, double)[rounds];

            Restriction.initializeZ(circles);

            var restrictions = Restriction.generate(circles, error);
            var limits = Limit.generate(restrictions, n);
            var poblation = Genetics.generatePoblation(limits, restrictions, size);

            for (int i = 0; i < rounds && i < 100; ++i){
                var best = round(poblation, limits, restrictions);

                int j;
                for (j = 0; j < best.Length; ++j)
                    poblation[j] = best[j];

                for (int k = j; k < poblation.Length; ++k){

                    while (err < limitOfExecution) {
                        err++;

                        if (random.Next(0, 2) == 0){
                            poblation[k] = mutation(best[random.Next(0, j)]);
                        }
                        else {
                            poblation[k] = crossover(best[random.Next(0, j)], best[random.Next(0, j)]);
                        }

                        if (evaluate(poblation[k], limits, restrictions))
                            break;
                    }

                    if (err == limitOfExecution)
                        throw new StackOverflowException("Limite de iteraciones alcanzado.");

                }

                var values = Genetics.getMappedValues(poblation[0], limits);

                answer[i] = (i, values[0], values[1], Restriction.z(values[0], values[1]));

                Console.WriteLine(answer[i].Item1);
                Console.WriteLine(answer[i].Item2);
                Console.WriteLine(answer[i].Item3);
                Console.WriteLine(answer[i].Item4);

            }

            return answer;
        }

        // Ronda de Geneticos
        public static char[][] round(char[][] poblation, Limit[] limits, Restriction[] restrictions)
        {

            double sum = 0;

            var z = new double[poblation.Length];
            for (int i = 0; i < z.Length; ++i){

                var values = Genetics.getMappedValues(poblation[i], limits);
                z[i] = -Restriction.z(values[0], values[1]);
                sum += z[i];
                
            }

            // Calcula el z porcentaje acumulado
            var zpercentage = new double[poblation.Length];
            var zacumulatePercentage = 0.0;
            for (int i = 0; i < z.Length; ++i){
                zpercentage[i] = z[i] / sum;
                zacumulatePercentage += zpercentage[i];
                zpercentage[i] = zacumulatePercentage;
            }

            var best = new PriorityQueue();
            for (int i = 0; i < z.Length; ++i){
                double r = random.NextDouble();
                for (int j = 0; j < z.Length; ++j){
                    if (r < zpercentage[j]){
                        best.push(poblation[j], z[j]);
                        break;
                    }
                }
            }

            return best.getOnlyValues();
        }

    }
}
