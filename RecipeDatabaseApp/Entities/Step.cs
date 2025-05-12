using System;
using System.Collections.Generic;

namespace RecipeDatabaseApp.Entities;

public partial class Step
{
    public int Stepid { get; set; }

    public int Recipeid { get; set; }

    public int Stepnumber { get; set; }

    public string Instruction { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
