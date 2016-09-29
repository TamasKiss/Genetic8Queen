using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic8Queens
{
    class Program
    {
        const int InitialPopulationSize = 8;
        const int NumOfQueens = 8;
        const double KeptPieceRatio = 0.5;
        const double MutationChance = 0.1;
        const int piecesMutated = 2;
        public static Random r = new Random();

        static void Main(string[] args)
        {
            List<string> population = initialPopulationGeneration();

            Console.WriteLine(Genetic_Algorithm(population));

            Console.ReadKey();
        }

        private static string Genetic_Algorithm(List<string> population)
        {
            List<int> fitnessOfPopulation = new List<int>();

            for (int j = 0; j < population.Count; j++)
                fitnessOfPopulation.Add(fitness_function(population[j]));

            for (int i = 0; fitnessOfPopulation.Max() != 28; i++)
            {

                int firstIndex = Array.IndexOf(fitnessOfPopulation.ToArray(), fitnessOfPopulation.Max());
                string first = population[firstIndex];
                int secondIndex = Array.IndexOf(fitnessOfPopulation.ToArray(), fitnessOfPopulation.Max());
                string second = population[secondIndex];

                string offspring = reproduction(first, second);

                if (MutationChance*10 > r.Next(0, 10))
                    offspring = mutation(offspring);

                population.Add(offspring);
                fitnessOfPopulation.Add(fitness_function(offspring));
            }

            Console.WriteLine("Population size: " + population.Count);
            return population[Array.IndexOf(fitnessOfPopulation.ToArray(), fitnessOfPopulation.Max())];
        }

        private static string mutation(string offspring)
        {
            char[] offsparray = offspring.ToCharArray();

            for (int i = 0; i < piecesMutated; i++)
                offsparray[r.Next(0, NumOfQueens)] = Convert.ToChar('0' + r.Next(1, 9));

            return new string(offsparray);
        }

        private static string reproduction(string first, string second)
        {
            double numOfKeptCharacters = first.Length * KeptPieceRatio;

            return first.Substring(0, (int) numOfKeptCharacters) + second.Substring((int) numOfKeptCharacters);
        }

        private static int fitness_function(string individual)
        {
            int fitness = 0;

            for (int i = 0; i < individual.Length-1; i++)
            {
                int piece = int.Parse(individual[i].ToString());

                for (int j = i+1; j < individual.Length; j++)
                {
                    int examined = int.Parse(individual[j].ToString());

                    if (piece == examined)
                        continue;

                    else if (piece + (j - i) == examined)
                        continue;

                    else if (piece - (j - i) == examined)
                        //Yes, this is could be condensed to a single if clause but I'm aiming for readibility
                        continue;

                    fitness++;
                }
            }
            return fitness;
        }

        private static List<string> initialPopulationGeneration()
        {
            List<string> population = new List<string>();

            for (int i = 0; i < InitialPopulationSize; i++)
            {
                string individual = "";

                for (int j = 0; j < NumOfQueens; j++)
                    individual += r.Next(1, 9);

                population.Add(individual);
            }

            return population;
        }
    }
}
