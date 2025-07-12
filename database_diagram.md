# Database Schema Diagram

## Entity Relationship Diagram

```mermaid
erDiagram
    NutritionAssessment {
        int id PK
        datetime created_at
        string name
        int age
        enum gender
        decimal weight
        decimal height
        int meals_per_day
        int vegetables_per_day
        int fruits_per_day
        int water_intake
        boolean eats_breakfast
        boolean eats_fast_food
        boolean eats_processed_food
        enum activity_level
        enum sleep_quality
        enum stress_level
    }

    Nutrient {
        int id PK
        string name
        string unit
        decimal daily_norm_min
        decimal daily_norm_max
    }

    NutrientBalance {
        int id PK
        int nutrition_assessment_id FK
        int nutrient_id FK
        decimal current_value
        decimal from_diet
        decimal from_supplements
        enum status
    }

    PersonalizedSet {
        int id PK
        int nutrition_assessment_id FK
        datetime created_at
        string name
        string description
    }

    Supplement {
        int id PK
        string name
        string description
        string image_url
        string dosage
        string when_to_take
    }

    NutritionAssessment ||--o{ NutrientBalance : "has"
    NutritionAssessment ||--o{ PersonalizedSet : "has"
    Nutrient ||--o{ NutrientBalance : "referenced_by"
    PersonalizedSet }o--o{ Supplement : "many-to-many"
```

## Database Tables

### NutritionAssessments
Stores user nutrition assessment data including personal information, dietary habits, and lifestyle factors.

### Nutrients
Contains reference data for various nutrients with their daily recommended values.

### NutrientBalances
Links nutrition assessments with specific nutrients, tracking current values and sources.

### PersonalizedSets
Custom supplement sets created for specific nutrition assessments.

### Supplements
Reference data for available supplements with dosage and usage information.