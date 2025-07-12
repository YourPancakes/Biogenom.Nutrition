using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Biogenom.Nutrition.Application.Services;
using Biogenom.Nutrition.Application.Interfaces;
using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Domain.Entities;
using Biogenom.Nutrition.Persistence.Context;
using Biogenom.Nutrition.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Biogenom.Nutrition.Tests;

public class NutritionAssessmentServiceTests
{
    [Fact]
    public async Task NutritionAssessmentService_CreateAssessmentAsync_CreatesAssessment()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        using var context = new ApplicationDbContext(options);
        var repository = new Repository<NutritionAssessment>(context);

        var nutrientBalanceServiceMock = new Mock<INutrientBalanceService>();
        nutrientBalanceServiceMock.Setup(x => x.CreateNutrientBalancesAsync(It.IsAny<NutritionAssessment>())).Returns(Task.CompletedTask);

        var personalizedSetServiceMock = new Mock<IPersonalizedSetService>();
        personalizedSetServiceMock.Setup(x => x.CreatePersonalizedSetAsync(It.IsAny<NutritionAssessment>())).Returns(Task.CompletedTask);

        var scoreCalculatorMock = new Mock<INutritionScoreCalculator>();
        scoreCalculatorMock.Setup(x => x.CalculateAssessmentScore(It.IsAny<CreateAssessmentRequest>())).Returns((100, Biogenom.Nutrition.Domain.Enums.NutritionQuality.Good));

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new Biogenom.Nutrition.Infrastructure.Mappers.NutritionAssessmentProfile());
        });
        var mapper = mapperConfig.CreateMapper();

        var loggerMock = new Mock<ILogger<NutritionAssessmentService>>();

        var service = new NutritionAssessmentService(
            repository,
            nutrientBalanceServiceMock.Object,
            personalizedSetServiceMock.Object,
            scoreCalculatorMock.Object,
            mapper,
            loggerMock.Object
        );

        var request = new CreateAssessmentRequest
        {
            Name = "Test User",
            Age = 30,
            Gender = Biogenom.Nutrition.Domain.Enums.Gender.Male,
            Weight = 70,
            Height = 175,
            MealsPerDay = 3,
            VegetablesPerDay = 2,
            FruitsPerDay = 1,
            WaterIntake = 5,
            EatsBreakfast = true,
            EatsFastFood = false,
            EatsProcessedFood = false,
            ActivityLevel = Biogenom.Nutrition.Domain.Enums.ActivityLevel.ModeratelyActive,
            SleepQuality = Biogenom.Nutrition.Domain.Enums.SleepQuality.Good,
            StressLevel = Biogenom.Nutrition.Domain.Enums.StressLevel.Low
        };

        var result = await service.CreateAssessmentAsync(request);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Test User", result.Value.Name);
        Assert.Equal(30, result.Value.Age);
        Assert.Equal(Biogenom.Nutrition.Domain.Enums.Gender.Male, result.Value.Gender);
    }
}
