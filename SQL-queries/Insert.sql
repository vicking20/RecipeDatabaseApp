-- Insert Units
INSERT INTO Unit (Name) VALUES ('grams'), ('ml'), ('tbsp');

-- Insert Ingredients
INSERT INTO Ingredient (Name) VALUES
('Flour'),
('Sugar'),
('Milk'),
('Egg'),
('Salt');

-- Insert Categories
INSERT INTO Category (Name) VALUES
('Dessert'),
('Breakfast'),
('Vegetarian');

-- Insert Recipes
INSERT INTO Recipe (Name, Description, PrepTime, CookTime, Servings) VALUES
('Pancakes', 'Fluffy breakfast pancakes', 10, 15, 2),
('Scrambled Eggs', 'Quick and easy eggs', 5, 5, 1),
('Milkshake', 'Cold blended milk drink', 5, 0, 1);

-- Insert Steps
-- Pancakes
INSERT INTO Step (RecipeID, StepNumber, Instruction) VALUES
(1, 1, 'Mix flour, sugar, and salt in a bowl.'),
(1, 2, 'Add milk and egg, then whisk until smooth.'),
(1, 3, 'Heat a pan and pour in batter. Cook until golden.');

-- Scrambled Eggs
INSERT INTO Step (RecipeID, StepNumber, Instruction) VALUES
(2, 1, 'Crack egg into a bowl and beat.'),
(2, 2, 'Pour into hot pan and stir gently.');

-- Milkshake
INSERT INTO Step (RecipeID, StepNumber, Instruction) VALUES
(3, 1, 'Blend milk, sugar, and ice until smooth.');

-- Insert RecipeIngredient
-- Pancakes
INSERT INTO RecipeIngredient (RecipeID, IngredientID, Quantity, UnitID) VALUES
(1, 1, 200, 1),  -- Flour, grams
(1, 2, 50, 1),   -- Sugar, grams
(1, 3, 300, 2),  -- Milk, ml
(1, 4, 1, 3);    -- Egg, tbsp

-- Scrambled Eggs
INSERT INTO RecipeIngredient (RecipeID, IngredientID, Quantity, UnitID) VALUES
(2, 4, 2, 3),    -- Eggs, tbsp
(2, 5, 1, 3);    -- Salt, tbsp

-- Milkshake
INSERT INTO RecipeIngredient (RecipeID, IngredientID, Quantity, UnitID) VALUES
(3, 3, 250, 2),  -- Milk, ml
(3, 2, 30, 1);   -- Sugar, grams

-- Insert RecipeCategory
INSERT INTO RecipeCategory (RecipeID, CategoryID) VALUES
(1, 1),  -- Pancakes → Dessert
(1, 2),  -- Pancakes → Breakfast
(2, 2),  -- Scrambled Eggs → Breakfast
(3, 1);  -- Milkshake → Dessert

-- Add Pancakes to Vegetarian (already Dessert + Breakfast)
INSERT INTO RecipeCategory (RecipeID, CategoryID) VALUES (1, 3);

-- Add Scrambled Eggs to Vegetarian (already Breakfast)
INSERT INTO RecipeCategory (RecipeID, CategoryID) VALUES (2, 3);

-- Add Milkshake to Vegetarian (already Dessert)
INSERT INTO RecipeCategory (RecipeID, CategoryID) VALUES (3, 3);
INSERT INTO RecipeCategory (RecipeID, CategoryID) VALUES (3, 2);

-- Pancakes
INSERT INTO RecipeIngredient (RecipeID, IngredientID, Quantity, UnitID) VALUES
(1, 1, 200, 1);  -- Flour, grams

-- Pancakes (RecipeID = 1)
INSERT INTO RecipeIngredient (RecipeID, IngredientID, Quantity, UnitID) VALUES
(1, 1, 200, 1),  -- Flour (grams)
(1, 2, 50, 1),   -- Sugar (grams)
(1, 3, 300, 2),  -- Milk (ml)
(1, 4, 1, 3),    -- Egg (tbsp)
(1, 5, 1, 3);    -- Salt (tbsp)

-- Scrambled Eggs (RecipeID = 2)
INSERT INTO RecipeIngredient (RecipeID, IngredientID, Quantity, UnitID) VALUES
(2, 4, 2, 3),    -- Egg (tbsp)
(2, 5, 1, 3);    -- Salt (tbsp)

-- Milkshake (RecipeID = 3)
INSERT INTO RecipeIngredient (RecipeID, IngredientID, Quantity, UnitID) VALUES
(3, 3, 250, 2),  -- Milk (ml)
(3, 2, 30, 1);   -- Sugar (grams)

