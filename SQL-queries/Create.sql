-- Recipe table
CREATE TABLE Recipe (
    RecipeID SERIAL PRIMARY KEY,
    Name TEXT NOT NULL,
    Description TEXT,
    PrepTime INT,
    CookTime INT,
    Servings INT
);

-- Ingredient table
CREATE TABLE Ingredient (
    IngredientID SERIAL PRIMARY KEY,
    Name TEXT NOT NULL UNIQUE
);

-- Unit table
CREATE TABLE Unit (
    UnitID SERIAL PRIMARY KEY,
    Name TEXT NOT NULL UNIQUE
);

-- Step table
CREATE TABLE Step (
    StepID SERIAL PRIMARY KEY,
    RecipeID INT NOT NULL,
    StepNumber INT NOT NULL,
    Instruction TEXT NOT NULL,
    FOREIGN KEY (RecipeID) REFERENCES Recipe(RecipeID) ON DELETE CASCADE
);

-- Category table
CREATE TABLE Category (
    CategoryID SERIAL PRIMARY KEY,
    Name TEXT NOT NULL UNIQUE
);

-- RecipeIngredient junction table
CREATE TABLE RecipeIngredient (
    RecipeID INT NOT NULL,
    IngredientID INT NOT NULL,
    Quantity DECIMAL(10, 2) NOT NULL,
    UnitID INT NOT NULL,
    PRIMARY KEY (RecipeID, IngredientID),
    FOREIGN KEY (RecipeID) REFERENCES Recipe(RecipeID) ON DELETE CASCADE,
    FOREIGN KEY (IngredientID) REFERENCES Ingredient(IngredientID) ON DELETE CASCADE,
    FOREIGN KEY (UnitID) REFERENCES Unit(UnitID)
);

-- RecipeCategory junction table
CREATE TABLE RecipeCategory (
    RecipeID INT NOT NULL,
    CategoryID INT NOT NULL,
    PRIMARY KEY (RecipeID, CategoryID),
    FOREIGN KEY (RecipeID) REFERENCES Recipe(RecipeID) ON DELETE CASCADE,
    FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID) ON DELETE CASCADE
);
