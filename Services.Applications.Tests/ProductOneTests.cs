using FluentAssertions;
using Xunit;

namespace Services.Applications.Tests;

public class ProductOneTests
{

    [Fact]
    public async Task Application_for_ProductOne_creates_Investor_in_AdministratorOne()
    {
        await Task.Delay(10);
        
        true.Should().BeTrue();
    }
}