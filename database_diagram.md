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

    PersonalizedSetSupplements {
        int personalized_set_id FK
        int supplement_id FK
    }

    NutritionAssessment ||--o{ NutrientBalance : "has"
    NutritionAssessment ||--o{ PersonalizedSet : "has"
    Nutrient ||--o{ NutrientBalance : "referenced_by"
    PersonalizedSet }o--o{ Supplement : "many-to-many"
    PersonalizedSet ||--o{ PersonalizedSetSupplements : "contains"
    Supplement ||--o{ PersonalizedSetSupplements : "included_in"
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

### PersonalizedSetSupplements
Junction table linking personalized sets with their recommended supplements.

## Enums

- **Gender**: Male, Female, Other
- **ActivityLevel**: Sedentary, LightlyActive, ModeratelyActive, VeryActive, ExtremelyActive
- **SleepQuality**: Poor, Fair, Good, Excellent
- **StressLevel**: Low, Moderate, High, VeryHigh
- **NutrientStatus**: Deficient, Insufficient, Adequate, Optimal, Excessive
- **NutritionQuality**: Poor, Fair, Good, Excellent 