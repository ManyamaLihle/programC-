using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeOptimizer
{
    class Program
    {
        // Ingredients
        static Dictionary<string, int> ingredients = new Dictionary<string, int>
        {
            { "Cucumber", 2 },
            { "Olives", 2 },
            { "Lettuce", 3 },
            { "Meat", 6 },
            { "Tomato", 6 },
            { "Cheese", 8 },
            { "Dough", 10 },
        };

        // Recipes
        class Recipe
        {
            public string Name { get; set; }
            public int Feeds { get; set; }
            public Dictionary<string, int> Requires { get; set; }
        }

        static List<Recipe> recipes = new List<Recipe>
        {
            new Recipe { Name = "Burger", Feeds = 1, Requires = new Dictionary<string, int> { {"Meat",1}, {"Lettuce",1}, {"Tomato",1}, {"Cheese",1}, {"Dough",1} } },
            new Recipe { Name = "Pie", Feeds = 1, Requires = new Dictionary<string, int> { {"Dough",2}, {"Meat",2} } },
            new Recipe { Name = "Sandwich", Feeds = 1, Requires = new Dictionary<string, int> { {"Dough",1}, {"Cucumber",1} } },
            new Recipe { Name = "Pasta", Feeds = 2, Requires = new Dictionary<string, int> { {"Dough",2}, {"Tomato",1}, {"Cheese",2}, {"Meat",1} } },
            new Recipe { Name = "Salad", Feeds = 3, Requires = new Dictionary<string, int> { {"Lettuce",2}, {"Tomato",2}, {"Cucumber",1}, {"Cheese",2}, {"Olives",1} } },
            new Recipe { Name = "Pizza", Feeds = 4, Requires = new Dictionary<string, int> { {"Dough",3}, {"Tomato",2}, {"Cheese",3}, {"Olives",1} } },
        };

        static void Main(string[] args)
        {
            var result = MaximizeFeed(ingredients, recipes);
            Console.WriteLine("Optimal combination:");
            foreach (var kvp in result.Item1)
            {
                if (kvp.Value > 0)
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine($"Total people fed: {result.Item2}");
        }

        // This is the combinations of  the max possible for each recipe
        static Tuple<Dictionary<string, int>, int> MaximizeFeed(
            Dictionary<string, int> available,
            List<Recipe> recipes)
        {
            // Calculate maximum ingredients
            var maxCounts = recipes.Select(r =>
                r.Requires.Select(req =>
                    available.ContainsKey(req.Key) ? available[req.Key] / req.Value : 0
                ).DefaultIfEmpty(0).Min()
            ).ToArray();

            int bestTotal = 0;
            Dictionary<string, int> bestCombo = null;

            // Try all combinations 
            for (int a = 0; a <= maxCounts[0]; a++)
            for (int b = 0; b <= maxCounts[1]; b++)
            for (int c = 0; c <= maxCounts[2]; c++)
            for (int d = 0; d <= maxCounts[3]; d++)
            for (int e = 0; e <= maxCounts[4]; e++)
            for (int f = 0; i <= maxCounts[5]; f++)
            {
                var combo = new[] { a, b, c, d, e, f };
                var used = new Dictionary<string, int>();
                bool valid = true;

                for (int i = 0; i < recipes.Count; i++)
                {
                    foreach (var req in recipes[i].Requires)
                    {
                        used[req.Key] = used.GetValueOrDefault(req.Key) + req.Value * combo[i];
                        if (used[req.Key] > available.GetValueOrDefault(req.Key, 0))
                            valid = false;
                    }
                }

                if (valid)
                {
                    int total = a * recipes[0].Feeds + b * recipes[1].Feeds +
                                c * recipes[2].Feeds + d * recipes[3].Feeds +
                                e * recipes[4].Feeds + f * recipes[5].Feeds;

                    if (total > bestTotal)
                    {
                        bestTotal = total;
                        bestCombo = new Dictionary<string, int>
                        {
                            { "Burger", a}, { "Pie", b }, { "Sandwich", c },
                            { "Pasta", d }, { "Salad", e }, { "Pizza", f }
                        };
                    }
                }
            }
            return Tuple.Create(bestCombo ?? new Dictionary<string, int>(), bestTotal);
        }
    }
}