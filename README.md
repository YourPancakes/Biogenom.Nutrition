# Biogenom.Nutrition

A comprehensive nutrition assessment and personalized supplement recommendation system built with .NET 8 and PostgreSQL.

## Overview

Biogenom.Nutrition is a sophisticated nutrition analysis platform that evaluates user dietary habits, lifestyle factors, and health metrics to provide personalized supplement recommendations. The system analyzes nutrient balances, identifies deficiencies, and creates customized supplement sets based on individual needs.

## Technology Stack

- **Backend**: .NET 8, Entity Framework Core
- **Database**: PostgreSQL
- **Architecture**: Clean Architecture with CQRS pattern
- **Validation**: FluentValidation
- **Documentation**: Swagger/OpenAPI
- **Containerization**: Docker & Docker Compose

## Getting Started

### Quick Start with Docker

1. **Clone the repository**
   ```bash
   git clone https://github.com/YourPancakes/Biogenom.Nutrition
   cd Biogenom.Nutrition
   ```

2. **Start the application**
   ```bash
   docker-compose up --build
   ```


## Access the application
   - API: http://localhost:5000
   - Swagger Documentation: http://localhost:5000/swagger
   - Database: localhost:5432 (PostgreSQL)
   
## Database Schema

The application uses a PostgreSQL database with the following main entities:

- **NutritionAssessment**: User assessment data including personal info, diet, and lifestyle
- **Nutrient**: Reference data for nutrients with daily recommended values
- **NutrientBalance**: Links assessments with nutrients, tracking current values
- **PersonalizedSet**: Custom supplement sets for specific assessments
- **Supplement**: Reference data for available supplements

For detailed database schema, see [database_diagram.md](database_diagram.md)

### API Endpoints

#### Nutrition Assessment
- `POST /api/v1/NutritionAssessment` - Create a new nutrition assessment
- `GET /api/v1/NutritionAssessment` - Get all assessments
- `GET /api/v1/NutritionAssessment/report/{assessmentId}` - Get assessment report
- `DELETE /api/v1/NutritionAssessment/{assessmentId}` - Delete assessment

#### Reports
- `GET /api/v1/report/current/last` - Get current nutrient report (latest assessment)
- `GET /api/v1/report/current/{assessmentId}` - Get nutrient report by assessment ID
- `GET /api/v1/report/personalized-set/last` - Get personalized set (latest assessment)
- `GET /api/v1/report/personalized-set/{assessmentId}` - Get personalized set by assessment ID