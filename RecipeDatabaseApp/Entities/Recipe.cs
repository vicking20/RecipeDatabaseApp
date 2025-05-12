using System;
using System.Collections.Generic;

namespace RecipeDatabaseApp.Entities;

public partial class Recipe
{
    public int Recipeid { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Preptime { get; set; }

    public int? Cooktime { get; set; }

    public int? Servings { get; set; }

    public virtual ICollection<Recipeingredient> Recipeingredients { get; set; } = new List<Recipeingredient>();

    public virtual ICollection<Step> Steps { get; set; } = new List<Step>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
