using Domain.Models;

namespace Domain.Test.Models;

public class CashFlowTest
{
    private readonly CashFlow _cashFlow;

    public CashFlowTest()
    {
        _cashFlow = CashFlowBuilder.New().Build();
    }

    [Fact]
    public void ShouldBeCreated()
    {
        // Arrange

        // Assert
        Assert.NotNull(_cashFlow);
        Assert.IsType<CashFlow>(_cashFlow);
    }

    [Fact]
    public void ShouldHaveUniqueId()
    {
        // Arrange
        var cashFlow2 = CashFlowBuilder.New().Build();

        // Assert
        Assert.NotEqual(_cashFlow.Id, cashFlow2.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ShouldNotAllowEmptyDescription(string description)
    {
        // Act
        void action() => CashFlowBuilder.New().WithDescription(description).Build();

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-100.00)]
    public void ShouldThrowForInvalidAmount(decimal amount)
    {
        // Act
        void action() => CashFlowBuilder.New().WithAmount(amount).Build();

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(13)]
    public void ShouldThrowForInvalidMonth(int month)
    {
        // Act
        void action() => CashFlowBuilder.New().WithMonth(month).Build();

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void ShouldThrowForInvalidYear()
    {
        // Arrange
        var years = new[] { 0, DateTime.Now.Year - 1 };

        foreach (var year in years)
        {
            // Act
            void action() => CashFlowBuilder.New().WithYear(year).Build();

            // Assert
            Assert.Throws<ArgumentException>(action);
        }
    }

    [Fact]
    public void ShouldHaveUserAssociation()
    {
        // Assert
        Assert.NotNull(_cashFlow.User);
    }

    [Fact]
    public void ShouldThrowsForNullUser()
    {
        // Act
        void action() => CashFlowBuilder.New().WithUser(null!).Build();

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void ShouldHaveCreatedAt()
    {
        // Assert
        Assert.NotEqual(default, _cashFlow.CreatedAt);
    }
}
