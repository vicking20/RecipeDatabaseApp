# **Project Report Template**

## **Title Page**

- **Project Name**: Recipe database project
- **Student Name(s)**:
- **Course/Module**: Databases 2025

---

## **Table of Contents**

1. [Step 1: Database Design](#step-1-database-design)
   1. [Schema Overview](#schema-overview)
   2. [Entities and Relationships](#entities-and-relationships)
   3. [Normalization & Constraints](#normalization--constraints)
   4. [Design Choices & Rationale](#design-choices--rationale)
2. [Step 2: Database Implementation](#step-2-database-implementation)
   1. [Table Creation](#table-creation)
   2. [Data Insertion](#data-insertion)
   3. [Validation & Testing](#validation--testing)
3. [Step 3: .NET Core Console Application Enhancement](#step-3-net-core-console-application-enhancement)
   1. [EF Core Configuration](#ef-core-configuration)
   2. [Implemented Features](#implemented-features)
   3. [Advanced Queries & Methods](#advanced-queries--methods)
   4. [Design Choices & Rationale](#design-choices--rationale-2)
4. [Challenges & Lessons Learned](#challenges--lessons-learned)
5. [Conclusion](#conclusion)

---

## **Step 1: Database Design**

### **Schema Overview**

- **High-Level Description**: Give a concise explanation of your schema’s purpose (e.g., to store recipes, ingredients, categories, etc.).
The purpose of this database schema is to manage and organize recipe-related data in a structured database. We can effectively store detailed data about:
- Recipes, preparation time, number of servings
- Ingredients and the quantity to use in each recipe
- Measurement units to standardize ingredient tracking
- Diet categories (eg. Dessert, vegetarian)
- Cooking instructions
- Relationships between recipes and ingredients, between recipes and categories.

**Entitiy**  -  **Description**
Recipe  -  Stores general information about the recipe
Ingredient  -  Stores individual ingredients
Category  -  Recipe categories
Step  -  Instrucitons for preparing the recipe
Unit  -  Measurement units (e.g grams, cups)

**Relationships**
A recipe can have many ingredients, and each ingredients can be used in many recipes Recipes -> Many-to-many ->
A recipe has many steps Steps -> One-to-many

Core tables:
Recipe -> RecipeID, Name, Description, PrepTime, CookTime, Servings
Ingredient -> IngredientID, Name
Category ->  CategoryID, Name
Unit -> UnitID, Name
Step -> StepID, RecipeID, StepNumber, Instruction
RecipeIngredient -> RecipeID, IngredientID, Quantity, UnitID
RecipeCategory -> RecipeID, CategoryID

**Normalization Strategies**
1NF: All cloumns hold atomic values
2Nf: Non-key columns are fully functionally dependent on the primary key
3NF: No transitive dependencies

**Constraints to apply**
Primary Key (PK) -> Available on all base tables
Foreign Key (FK) -> Available for example on Recipe Ingredients, Recipe Category, Step
Unique -> Available on fields like Category.Name, Ingredient.Name
Not Null -> On essential columns (Recipe.Name, Step.Instruction)

- **Diagram**: Insert or reference your Entity-Relationship Diagram (ERD) here. You can include it in this section.

### **Entities and Relationships**

- **List of Entities**: Describe each table (e.g., `Recipe`, `Ingredient`), including their main attributes.
1. Recipe : RecipeID(PK), Name, Description, PrepTime, CookTime, Servings

2. Ingredient : IngredientID(PK), Name

3. Unit : UnitID(PK), Name

4. Step : StepID(PK), RecipeID(FK), StepNumber, Instruction

5. Category : CategoryID(PK), Name

6. RecipeIngredient(junction table) : RecipeID(FK, PK), IngredientID(FK,PK), Quantity, UnitID(FK)

7. RecipeCategory(junction table) : RecipeID(FK, PK), CategoryID(FK, PK)

- **Relationship Descriptions**: Explain the relationships (one-to-many, many-to-many, etc.) and how they are represented (junction tables, foreign keys, etc.).
Recipe - Step (One-to-many(1:N)): A single recipe can have many steps. Step references RecipeID as a foreign key
Recipe - Ingredient (Many-to-Many(M:N)): A recipe uses many ingredients, and an ingredient can be used in many recipes. This is modeled using the RecipeIngredient table
Recipe - category (Many-to-Many(M:N)): A recipe can belong to multiple categories (e.g vegan, dinner), and each category includes multiple recipes. This is handled with the RecipeCategory junction table
Unit - RecipeIngredient (One-to-many(1:N)): A unit (e.g grams, teaspoons) can be used by many recipe-ingredient entries 

### **Normalization & Constraints**

- **Normalization Level**: State the level of normalization (e.g., 3NF) you aimed for and why.
This schema is designed to satisfy third normal form 3NF where there are no transitive dependencies; all non key attributes are only dependent on the key. This ensures data integgrity, minimizes redundancy and improves scalability

- **Constraints**: Discuss your use of primary keys, foreign keys, `NOT NULL`, `UNIQUE`, etc.
Primary Keys: Uniquely identify records in each table.

Foreign Keys: Maintain referential integrity between related tables.

NOT NULL: Enforced on required fields like Recipe.Name or Step.Instruction.

UNIQUE: Applied where necessary to prevent duplicates (e.g., Ingredient.Name and Category.Name).

Composite Primary Keys: Used in RecipeIngredient and RecipeCategory to avoid duplicate entries.

### **Design Choices & Rationale**

- **Reasoning**: Justify **why** you structured the schema the way you did. For instance, “We used a junction table for Recipe-Ingredient because it’s a many-to-many relationship.”
I used junction tables (RecipeIngredient, RecipeCategory) to handle many-to-many relationships.

A separate Unit table was included to normalize units instead of storing them as plain text (e.g., “grams”, “ml”)—this improves consistency and reduces typos.

Steps are stored in a separate table with a StepNumber to maintain order and allow editing of individual instructions.

- **Alternatives Considered**: Note any alternative designs you evaluated and why you chose not to implement them.
I considered storing ingredients and quantities directly in the Recipe table, but this would violate normalization and make querying more complex.

Another alternative was embedding categories as a comma-separated list in Recipe, but this violates 1NF and makes filtering by category inefficient.

---

## **Step 2: Database Implementation**

### **Table Creation**

- **SQL Scripts Overview**: Provide or reference your `CREATE TABLE` statements.
- **Explanation of Key Fields**: For each table, briefly explain the most important columns and their data types.
- **Constraints**: Show how you implemented the constraints (e.g., `PRIMARY KEY`, `FOREIGN KEY`, etc.) in SQL.

### **Data Insertion**

- **Sample Data**: Summarize the sample data you inserted. For example, 5 ingredients, 3 recipes, multiple categories, etc.
All CREATE TABLE statements are written in the SQL-queries/Create.sql file in the project. Each table was created with necessary fields, data types and constraints for a normalized relational model.

Key fields and data types
Table - Important fields - Notes
Recipe - RecipeID(PK, serial), Name(Text) - Core entity that stores metadata about each recipe
Ingredient - IngredientID(PK), Name(Text) - Ingredeient names are unique and can be used in many recipes
Unit - UnitID(PK), Name(Text) - Defines measurement units (e.g grams)
Step - StepID(PK), RecipeID(FK), Instruction - Ordered steps by recipe using StepNumber for definition
Category - CategoryID(PK), Name(Text) - Recipe categories
RecipeIngredient - Composes of RecipeID + IngredientID - Junction table storing Quantity and UnitID
RecipeCategory - Composes of RecipeID + CategoryID - Junction table to implement many to many models for recipes and categories

- **Constraints Used**
'Primary' and 'foreign' keys used to ensure data integrity
'Not Null' used for constraints on essential fields (e.g names, step instructions)
'Unique' constraints on Ingredient.Name and Category.Name
Composite 'Primary' keys in RecipeIngredient and RecipeCategory prevent duplicates

- **INSERT Statements**: Provide or reference your data insertion scripts (`INSERT INTO ...`).
Sample data summary
The SQL-queries/Insert.sql file contains all INSERT INTO statements

Table - Rows Inserted
Recipe - 3 recipes
Ingredient - 5 ingredients
Unit - 3 units
Step - 7 steps
Category - 3 categories
RecipeIngredient - 6 entries
RecipeCategory - 4 entries

Each recipe has:
2+ Ingredients
Ordered preparation steps
One or more categories

### **Validation & Testing**

- **Basic Queries**: Document a few test queries you ran using `psql` or another tool (e.g. `pgAdmin`) to confirm your data was inserted correctly.
select * from recipe;

"SELECT r.Name AS Recipe, s.StepNumber, s.Instruction
FROM Recipe r
JOIN Step s ON r.RecipeID = s.RecipeID
ORDER BY r.Name, s.StepNumber;
"


- **Results**: Summarize the outcome (e.g., “Query shows 3 recipes in the `Recipe` table. Each has multiple entries in `RecipeIngredient`. No foreign key violations.”)
First query shows all data in recipe table, we have 3 items and they have their primary key which is the recipe id, their name, description, prep time, cook time and how many servings

The second query shows recipes, the steps involved, the instructions to prepare from each steps

---

## **Step 3: .NET Core Console Application Enhancement**

### **EF Core Configuration**

- **Connection String**: Describe where and how you manage the database connection string (e.g., `appsettings.json`, environment variables).


- **DbContext**: Summarize your e.g. `RecipeDbContext` setup, how you map entities.
This DbContext connects to a PostgreSQL database (recipe_db) and manages entities involved in a recipe application: Recipe, Ingredient, Step, Category, Unit, and junction entities like RecipeIngredient and RecipeCategory.

Entity Mappings
Recipe

    Table: recipe

    Primary Key: recipeid

    Has many:

        Steps

        RecipeIngredients

        Categories (many-to-many via recipecategory)

Category

    Table: category

    Primary Key: categoryid

    Unique: name

    Many-to-many with Recipes via recipecategory

Ingredient

    Table: ingredient

    Primary Key: ingredientid

    Unique: name

    Related to Recipes via recipeingredient (many-to-many)

Unit

    Table: unit

    Primary Key: unitid

    Unique: name

    Used in RecipeIngredient to describe the quantity

Step

    Table: step

    Primary Key: stepid

    Belongs to one Recipe (via recipeid)

    Includes stepnumber and instruction

RecipeIngredient (junction table)

    Table: recipeingredient

    Composite Primary Key: { recipeid, ingredientid }

    Contains: quantity, unitid

    Foreign Keys:

        recipeid → recipe

        ingredientid → ingredient

        unitid → unit

RecipeCategory (junction table)

    Table: recipecategory

    Composite Primary Key: { recipeid, categoryid }

    Many-to-many mapping using EF's UsingEntity<Dictionary<string, object>>


### **Implemented Features**

- **CRUD Operations**: Explain your approach for Create, Read, Update, and Delete methods (e.g., adding new recipes, listing all recipes).
Implemented Features
CRUD Operations

    Create: New recipes can be added to the database.

    Read: Users can:

        List all recipes with associated metadata.


    Update: Recipes can be updated via ID.

    Delete: Recipes be safely removed from database.

Advanced Features

    Search Recipes by Ingredients: Users can enter one or more ingredient names; only recipes that contain all specified ingredients are returned.

    Fetch Recipes by Category: Retrieves all recipes assigned to a given category.

    Add/Remove Category from Recipe: Users can dynamically associate or disassociate categories with recipes.

    Ingredient and Unit Linking: Ingredients can be linked with quantities and measurement units for each recipe.

    Unique Constraints Handling: Ingredients, units, and categories use HasIndex(...).IsUnique() to avoid duplicates.

- **Advanced Features**: List any advanced features such as searching by multiple ingredients or retrieving recipes by category.

### **Advanced Queries & Methods**

- **LINQ Queries**: Show or summarize the LINQ queries used for more complex requirements (e.g., “Fetch recipes containing all specified ingredients”).
- **Performance Considerations**: If relevant, mention any indexes or optimizations you added.

---

## **Challenges & Lessons Learned**

- **Obstacles Faced**: Mention any difficulties you encountered (e.g., configuring EF Core, handling many-to-many relationships).
- **Key Takeaways**: Summarize the primary lessons you learned about database design, SQL, and .NET Core development.

---

## **Conclusion**

- **Project Summary**: Recap the final state of the project, including major accomplishments (e.g., fully functioning console app integrated with your Postgres database).
- **Future Enhancements**: Suggest any next steps or improvements you would make if you had more time (e.g., add user authentication, implement rating systems, or expand the domain).

---

### **Instructions for Use**

1. **Fill Out Each Section**: Provide clear, concise, and **original** explanations.
2. **Include Screenshots or Snippets**: If it helps clarify a point (e.g., menu output from the console app, partial code snippets).
3. **Maintain Professional Formatting**: Use consistent headers, bullet points, and references.
