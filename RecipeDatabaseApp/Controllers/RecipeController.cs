//RecipeController.cs
using Microsoft.EntityFrameworkCore;
using RecipeDatabaseApp.Entities;

namespace RecipeDatabaseApp.Controllers
{
    public class RecipeController
    {
        // Update the DbContext to match your dbContext, e.g. WebStoreContext
        private readonly DbContext _dbContext;

        // Update the DbContext to match your dbContext, e.g. WebStoreContext
        public RecipeController(DbContext context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Retrieves all recipes from the database and prints them to the console.
        /// Implementation should use EF Core to fetch Recipe entities
        /// and display relevant fields (e.g., ID, Name).
        /// </summary>
        public async Task ListAllRecipes() 
        {
            var recipes = await _dbContext.Set<Recipe>().ToListAsync();
            if (recipes.Count == 0)
            {
                Console.WriteLine("There were no recipes found!");
                return;
            }
            // Print out all Recipes
            Console.WriteLine("Recipe lists");
            foreach (var recipe in recipes)
            {
                Console.WriteLine($"ID: {recipe.Recipeid}, Name: {recipe.Name}, Description: {recipe.Description}");
            }
        }

        /// <summary>
        /// Associates an existing Category with a specified Recipe,
        /// based on user input (e.g., recipe ID/name and category name).
        /// The method should validate that both Recipe and Category
        /// exist, then create the necessary relationship in the database.
        /// </summary>
        internal async Task AddCategoryToRecipe()
        {
            await ListAllRecipes();
            Console.WriteLine("Enter recipe number (id) to add a category to: ");

            if (!int.TryParse(Console.ReadLine(), out int recipeId))
            {
                Console.WriteLine("Invalid Recipe ID.");
                return;
            }

            var recipe = await _dbContext.Set<Recipe>()
                .Include(r => r.Categories)
                .FirstOrDefaultAsync(r => r.Recipeid == recipeId);

            if (recipe == null)
            {
                Console.WriteLine($"No recipe found with ID {recipeId}.");
                return;
            }

            await ListAllCategories();

            //Prompt user to enter category name
            Console.Write("Enter the Category name to associate (e.g., Dessert): ");
            string? categoryName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                Console.WriteLine("Category name cannot be empty.");
                return;
            }

            //Retreive the category
            var category = await _dbContext.Set<Category>()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());

            if (category == null)
            {
                Console.WriteLine($"No category found with name '{categoryName}'.");
                return;
            }

            // Check if already associated
            if (recipe.Categories.Any(c => c.Categoryid == category.Categoryid))
            {
                Console.WriteLine($"The recipe '{recipe.Name}' already belongs to the '{category.Name}' category.");
                return;
            }

            // Add the category to the recipe
            recipe.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            Console.WriteLine($"Category '{category.Name}' added to recipe '{recipe.Name}'.");
        }

        //Added category list to show categories to users when for example adding category to recipe
        internal async Task ListAllCategories()
        {
            var categories = await _dbContext.Set<Category>().ToListAsync();
            if (categories.Count == 0)
            {
                Console.WriteLine("There were no categories found!");
                return;
            }
            // Print out all categories
            Console.WriteLine("Category list");
            foreach (var category in categories)
            {
                Console.WriteLine($"ID: {category.Categoryid}, Name: {category.Name}");
            }
        }

        /// <summary>
        /// Allows the user to add a new Ingredient to the database,
        /// specifying properties such as Name, Type, and any optional
        /// nutritional details. Should use EF Core to create and
        /// save the new Ingredient entity.
        /// </summary>
        internal async Task AddNewIngredient()
        {
            Console.WriteLine("Enter Name of Ingredient: ");
            string? name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Ingredient name cannot be empty!");
            return;
        }
        var ingredient = new Ingredient
        {
            Name = name.Trim()
        };

        _dbContext.Set<Ingredient>().Add(ingredient);
        await _dbContext.SaveChangesAsync();

        Console.WriteLine("Ingredient added successfully.");

        }

        /// <summary>
        /// Creates a new Recipe entry by prompting the user for
        /// recipe details (name, description, etc.). 
        /// Implementation should add a new Recipe entity via EF Core
        /// and save changes to the database.
        /// </summary>
        internal async Task AddNewRecipe()
        {
            Console.WriteLine("Write recipe name: ");
            string name = Console.ReadLine();

            Console.WriteLine("Write recipe description: ");
            string description = Console.ReadLine();

            Console.WriteLine("Write the preparation time (in minutes): ");
            int preptime = int.Parse(Console.ReadLine());

            Console.WriteLine("Write the cook time (in minutes): ");
            int cooktime = int.Parse(Console.ReadLine());

            Console.WriteLine("Write how many servings: ");
            int servings = int.Parse(Console.ReadLine());

            var recipe = new Recipe
            {
                Name = name,
                Description = description,
                Preptime = preptime,
                Cooktime = cooktime,
                Servings = servings

            };
            await _dbContext.Set<Recipe>().AddAsync(recipe);
            await _dbContext.SaveChangesAsync();

            Console.WriteLine("Recipe added successfully!");
        }

        /// <summary>
        /// Removes an existing Recipe from the database by prompting
        /// the user for a Recipe identifier (e.g., ID or name).
        /// Should handle deletion of any related data (e.g., from
        /// RecipeIngredient junction tables) if cascades are not enabled.
        /// </summary>
        internal async Task DeleteRecipe()
        {
            await ListAllRecipes();

            Console.WriteLine("Select Recipe to delete by its ID (number): ");
            if (!int.TryParse(Console.ReadLine(), out int recipeId))
            {
                Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
                return;
            }

            var recipe = await _dbContext.Set<Recipe>()
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Recipeid == recipeId);
            
            if (recipe == null)
            {
                Console.WriteLine($"No recipe found with ID {recipeId}.");
                return;
            }

            _dbContext.Set<Recipe>().Remove(recipe);
            await _dbContext.SaveChangesAsync();
            Console.WriteLine($"Recipe with ID {recipeId} has been deleted.");

        }

        /// <summary> 
        /// Fetches all recipes under a specified category by prompting
        /// the user for the category name. Uses EF Core and LINQ
        /// to filter recipes belonging to that category, then prints 
        /// them to the console.
        /// </summary>
        internal async Task FetchRecipeByCategory()
        {
            Console.WriteLine("Enter category name to search recipes (e.g., Dessert, Breakfast): ");
            string? categoryName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                Console.WriteLine("Category name cannot be empty.");
                return;
            }

            var recipes = await _dbContext.Set<Recipe>()
                .Include(r => r.Categories)
                .Where(r => r.Categories.Any(c => c.Name.ToLower() == categoryName.ToLower()))
                .ToListAsync();

            if (recipes.Count == 0)
            {
                Console.WriteLine($"No recipes found in the '{categoryName}' category.");
                return;
            }

            Console.WriteLine($"\nRecipes in the '{categoryName}' category:");
            foreach (var recipe in recipes)
            {
                Console.WriteLine($"- {recipe.Name} (ID: {recipe.Recipeid})");
            }
        }

        /// <summary>
        /// Removes a given Category association from a Recipe.
        /// The method should confirm both entities exist, then remove
        /// their relationship in the junction table or foreign key.
        /// </summary>
        internal async Task RemoveCategoryFromRecipe()
        {
            await ListAllRecipes();
            //prompt for recipe id
            Console.Write("Enter the recipe number (ID) to remove a category from: ");

            if (!int.TryParse(Console.ReadLine(), out int recipeId))
            {
                Console.WriteLine("Invalid Recipe ID.");
                return;
            }
            //Retreive recipe with categories
            var recipe = await _dbContext.Set<Recipe>()
                .Include(r => r.Categories)
                .FirstOrDefaultAsync(r => r.Recipeid == recipeId);

            if (recipe == null)
            {
                Console.WriteLine($"No recipe found with ID {recipeId}.");
                return;
            }
            //Prompt for category name
            await ListAllCategories();

            Console.Write("Enter the Category name to remove (e.g., Dessert): ");
            string? categoryName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                Console.WriteLine("Category name cannot be empty.");
                return;
            }

            // Find the category in the recipe’s category list
            var categoryToRemove = recipe.Categories
                .FirstOrDefault(c => c.Name.ToLower() == categoryName.ToLower());

            if (categoryToRemove == null)
            {
                Console.WriteLine($"The recipe '{recipe.Name}' is not associated with the category '{categoryName}'.");
                return;
            }

            // Remove the category from the recipe
            recipe.Categories.Remove(categoryToRemove);
            await _dbContext.SaveChangesAsync();

            Console.WriteLine($"Category '{categoryToRemove.Name}' removed from recipe '{recipe.Name}'.");

        }

        /// <summary>
        /// Searches for recipes containing all of the user-specified
        /// ingredients. The user can input multiple ingredient names;
        /// the method should return only recipes that include
        /// all those ingredients.
        /// </summary>
        internal async Task SearchRecipeByIngredients()
        {
            Console.WriteLine("Enter ingredient names separated by commas (e.g., egg, milk, sugar):");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("No ingredients provided.");
                return;
            }

            var ingredientNames = input.Split(',')
                                        .Select(i => i.Trim().ToLower())
                                        .Where(i => !string.IsNullOrEmpty(i))
                                        .ToList();

            if (ingredientNames.Count == 0)
            {
                Console.WriteLine("No valid ingredients entered");
                return;
            }

            //To find matching ingredient ids
            var ingredientIds = await _dbContext.Set<Ingredient>()
                .Where(i => ingredientNames.Contains(i.Name.ToLower()))
                .Select(i => i.Ingredientid)
                .ToListAsync();

            if (ingredientIds.Count != ingredientNames.Count)
            {
                Console.WriteLine("Some ingredients were not found in the database.");
                return;
            }

            // Find recipes that contain ALL of the specified ingredients
            var recipes = await _dbContext.Set<Recipe>()
                .Where(r => ingredientIds.All(id =>
                    r.Recipeingredients.Any(ri => ri.Ingredientid == id)))
                .ToListAsync();

            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes found with all the specified ingredients.");
                return;
            }

            Console.WriteLine("\nRecipes containing all specified ingredients:");
            foreach (var recipe in recipes)
            {
                Console.WriteLine($"- {recipe.Name} (ID: {recipe.Recipeid})");
            }

        }

        /// <summary>
        /// Updates fields of an existing Recipe, e.g., Name, Description,
        /// or other metadata. Prompts the user for a Recipe identifier,
        /// retrieves the entity from the database, modifies fields,
        /// and saves changes back to the database.
        /// </summary>
        internal async Task UpdateRecipe()
        {
            await ListAllRecipes();
            
            Console.WriteLine("Select Recipe to update by its ID (number): ");
            if (!int.TryParse(Console.ReadLine(), out int recipeId))
            {
                Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
                return;
            }

            var recipe = await _dbContext.Set<Recipe>().FirstOrDefaultAsync(r => r.Recipeid == recipeId);
            if (recipe == null)
            {
                Console.WriteLine($"No recipe found with ID {recipeId}.");
                return;
            }

            Console.WriteLine("\nSelect the number for the option you want to update: ");   
            Console.WriteLine("1. Name");
            Console.WriteLine("2. Description");
            Console.WriteLine("3. Prep Time");
            Console.WriteLine("4. Cook Time");
            Console.WriteLine("5. Servings");
            Console.WriteLine("Choose: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter new name: ");
                    recipe.Name = Console.ReadLine();
                    break;

                case "2":
                    Console.Write("Enter new description: ");
                    recipe.Description = Console.ReadLine();
                    break;

                case "3":
                    Console.Write("Enter new Prep Time (in minutes): ");
                    if (int.TryParse(Console.ReadLine(), out int prepTime))
                        recipe.Preptime = prepTime;
                    else
                        Console.WriteLine("Invalid input. Prep time not changed.");
                    break;

                case "4":
                    Console.Write("Enter new Cook Time (in minutes): ");
                    if (int.TryParse(Console.ReadLine(), out int cookTime))
                        recipe.Cooktime = cookTime;
                    else
                        Console.WriteLine("Invalid input. Cook time not changed.");
                    break;

                case "5":
                    Console.Write("Servings: ");
                    if (int.TryParse(Console.ReadLine(), out int servings))
                        recipe.Servings = servings;
                    else
                        Console.WriteLine("Invalid input. Servings not changed.");
                    break;

                default:
                Console.WriteLine("Invalid choice. No updates made.");
                return;
            }
            await _dbContext.SaveChangesAsync();
            Console.WriteLine("Recipe updated successfully.");
        }
    }
}