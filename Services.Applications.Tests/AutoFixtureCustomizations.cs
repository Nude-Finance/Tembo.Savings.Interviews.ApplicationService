using AutoFixture;
using AutoFixture.Xunit2;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Tests;

public class DateOfBirthAutoDataAttribute() 
    : AutoDataAttribute(() => new Fixture().Customize(new DateOfBirthCustomization()));

public class DateOfBirthCustomization : ICustomization
{
    private readonly Random _randomizer = new();

    public void Customize(IFixture fixture)
    {
        fixture.Customize<User>(composer => composer.With(user => user.DateOfBirth, RandomDateOfBirth));
    }

    /// <summary>
    /// Instantiates a DateOnly object for a representative date of birth between 10 and 100 years old 
    /// </summary>
    /// <returns>DateOnly</returns>
    private DateOnly RandomDateOfBirth()
    {
        var today = DateTime.Today;
        var year = _randomizer.Next(today.Year - 100, today.Year - 9);
        var days = _randomizer.Next(0, 365);
        var result = new DateOnly(year, 01, 01);
        result.AddDays(days);

        return result;
    }
}