using System;
using System.Collections.Generic;
using System.Linq;

namespace Genetic8Queens
{
    class Program
    {
        const int InitialPopulationSize = 8;
        const int NumOfQueens = 8;
        const double KeptPieceRatio = 0.5;
        const double MutationChance = 0.01;
        const int PiecesMutated = 2;
        public static Random r = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine(Genetic_Algorithm());

            Console.ReadKey();
        }

        private static int Genetic_Algorithm()
        {
            List<int> population = InitialPopulationGeneration();

            List<int> fitnessOfPopulation = new List<int>();

            for (int j = 0; j < population.Count; j++)
                fitnessOfPopulation.Add(fitness_function(population[j]));

            while(fitnessOfPopulation.Max() != 28)
            {

                int firstIndex = Array.IndexOf(fitnessOfPopulation.ToArray(), fitnessOfPopulation.Max());
                int first = population[firstIndex];
                int secondIndex = Array.IndexOf(fitnessOfPopulation.ToArray(), fitnessOfPopulation.Max());
                int second = population[secondIndex];

                int offspring = Reproduction(first, second);

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
                offsparray[r.Next(0, NumOfQueens)] = Convert.ToChar('0' + r.Next(1, 9));

            return int.Parse(new string(offsparray));
        }

        private static int Reproduction(int first, int second)
        {
            int digits = Convert.ToInt32(Math.Floor(Math.Log10(first) + 1));

            double numOfKeptCharacters = digits * KeptPieceRatio;

            return int.Parse(first.ToString().Substring(0, (int) numOfKeptCharacters) + second.ToString().Substring((int) numOfKeptCharacters));
        }

        private static int fitness_function(int individual)
        {
            int fitness = 0;
            int digits = Convert.ToInt32(Math.Floor(Math.Log10(individual) + 1));

            for (int i = 0; i < digits-1; i++)
            {
                int piece = (int)(individual.ToString()[i]) - 48;

                for (int j = i+1; j < digits; j++)
                {
                    int examined = (int)(individual.ToString()[j]) - 48;

                    if (piece == examined)
                        continue;

                    else if (examined + (j - i) == piece)
                        continue;

                    else if (examined - (j - i) == piece)
                        //Yes, this is could be condensed to a single if clause but I'm aiming for readibility
                        continue;

                    fitness++;
                }
            }
            return fitness;
        }

        private static List<int> InitialPopulationGeneration()
        {
            List<int> population = new List<int>();

            for (int i = 0; i < InitialPopulationSize; i++)
            {
                int individual = 0;
                int unit = 1;

                for (int j = 0; j < NumOfQueens; j++)
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
