using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Genetic8Queens
{
    class Program
    {
        static double KeptPieceRatio = 0.5;
        static double MutationChance = 0.06;
        static int PiecesMutated = 2;
        static Random r = new Random();

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            Parallel.For((long) 0, 10, index => { Console.WriteLine(index + " " + Genetic_Algorithm()); });
            sw.Stop();

            Console.WriteLine("Total Time: " + sw.ElapsedMilliseconds + "ms");
            Console.ReadKey();
        }

        private static int Genetic_Algorithm()
        {
            List<int> population = GenerateInitialPopulation();

            List<int> fitnessOfPopulation = population.Select(fitness_function).ToList(); //Calculating fitness for each instance

            while(fitnessOfPopulation.Max() != 28) //When we have the best possible fitness, we have a solution
            {
                int indexOfBest = Array.IndexOf(fitnessOfPopulation.ToArray(), fitnessOfPopulation.Max());
                int bestInstance = population[indexOfBest];
                int indexOfSecond = Array.IndexOf(fitnessOfPopulation.ToArray(), fitnessOfPopulation.Max());
                int second = population[indexOfSecond];

                int offspring = Reproduction(bestInstance, second);

                if (MutationChance*100 > r.Next(0, 101))
                    offspring = Mutation(offspring);

                if (!population.Contains(offspring))
                {
                    population.Add(offspring);
                    fitnessOfPopulation.Add(fitness_function(offspring));
                }
            }

            Console.WriteLine("Population size: " + population.Count);
            return population[Array.IndexOf(fitnessOfPopulation.ToArray(), fitnessOfPopulation.Max())];
        }

        private static int Mutation(int offspring)
        {
            char[] offsparray = offspring.ToString().ToCharArray();

            for (int i = 0; i < PiecesMutated; i++)
                offsparray[r.Next(0, 8)] = Convert.ToChar('0' + r.Next(1, 9));

            return int.Parse(new string(offsparray));
        }

        private static int Reproduction(int first, int second)
        {
            int numOfKeptCharacters = (int)(8 * KeptPieceRatio);

            return int.Parse(first.ToString().Substring(0, numOfKeptCharacters) + second.ToString().Substring(numOfKeptCharacters));
        }

        private static int fitness_function(int individual)
        {
            int fitness = 0;

            for (int i = 0; i < 7; i++) // <Number of columns> - 1 = 7
            {
                int piece = (individual.ToString()[i]) - 48;

                for (int j = i+1; j < 8; j++)
                {
                    int examined = (individual.ToString()[j]) - 48;

                    if (piece == examined)
                        continue;

                    if (examined + (j - i) == piece)
                        continue;

                    if (examined - (j - i) == piece)
                        continue;

                    fitness++;
                }
            }
            return fitness;
        }

        private static List<int> GenerateInitialPopulation()
        {
            List<int> population = new List<int>();

            for (int i = 0; i < 8; i++)
            {
                int individual = 0;
                int unit = 1;

                for (int j = 0; j < 8; j++)
                {
                    individual += r.Next(1, 9)*unit;
                    unit *= 10;
                }
                population.Add(individual);
            }

            return population;
        }
    }
}
