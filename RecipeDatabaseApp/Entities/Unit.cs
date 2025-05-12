using System;
using System.Collections.Generic;

namespace RecipeDatabaseApp.Entities;

public partial class Unit
{
    public int Unitid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Recipeingredient> Recipeingredients { get; set; } = new List<Recipeingredient>();
}
