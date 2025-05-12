//program.cs
using System;
using System.IO;
using System.Threading.Tasks; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecipeDatabaseApp.Controllers;
using RecipeDatabaseApp;
using RecipeDatabaseApp.Entities;

namespace RecipeDatabaseApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Build configuration to read app setitngs json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var connectionString = config.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            //Database context initialization
            using var dbContext = new RecipeDbContext(options);
            
            //run menu
            await RunMenu(dbContext);
            // 1. Initialize your database context here
            // e.g using var dbContext = new WebStoreContext();

            // 2. Pass the context to the RunMenu method to run a simple menu loop
            //RunMenu(dbContext);
        }

        /// <summary>
        /// Simple method that generates terminal menu that can be used to interact with the database
        /// </summary>
        private static async Task RunMenu(DbContext dbContext)
        {
            var recipeController = new RecipeController(dbContext);

            bool exit = false;
            while (!exit)
            {
                try
                {

                    Console.WriteLine("\n=== Recipe Database App ===");
                    Console.WriteLine("1. List All Recipes");
                    Console.WriteLine("2. Add New Recipe");
                    Console.WriteLine("3. Update Recipe");
                    Console.WriteLine("4. Delete Recipe");
                    Console.WriteLine("5. Fetch Recipes by Category");
                    Console.WriteLine("6. Search Recipes by Ingredients");
                    Console.WriteLine("7. Add Category to Recipe");
                    Console.WriteLine("8. Remove Category from Recipe");
                    Console.WriteLine("9. Extra option");
                    Console.WriteLine("0. Exit");
                    Console.Write("Select an option: ");
                    var input = Console.ReadLine();

                    // Use the case "1" as an example to implement other features
                    // (change the number to match correct option) 
                    switch (input)
                    {
                        case "1":
                            await recipeController.ListAllRecipes();
                            break;
                        case "2":
                            await recipeController.AddNewRecipe();
                            break;
                        case "3":
                            await recipeController.UpdateRecipe();
                            break;
                        case "4":
                            await recipeController.DeleteRecipe();
                            break;
                        case "5":
                            await recipeController.FetchRecipeByCategory();
                            break;
                        case "6":
                            await recipeController.SearchRecipeByIngredients();
                            break;
                        case "7":
                            await recipeController.AddCategoryToRecipe();
                            break;
                        case "8":
                            await recipeController.RemoveCategoryFromRecipe();
                            break;
                        case "9":
                            Console.WriteLine("Extra option not implemented yet.");
                            break;
                        case "0":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Try again.");
                            break;
                    }
                    Console.WriteLine("");
                    Console.WriteLine("============== ************* ==============");
                    Console.WriteLine("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something went wrong: " + ex.Message);
                }

            }
        }

        private static void ListAllRecipes()
        {
            throw new NotImplementedException();
        }
    }
}
