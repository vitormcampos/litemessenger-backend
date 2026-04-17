using Domain.Models;

namespace Domain.Test.Models;

public class CashFlowTest
{
    private readonly CashFlow _cashFlow;
    private readonly User _user;

    public CashFlowTest()
    {
        _user = new User("testuser", "testuser@example.com", "hashedpassword");
        _cashFlow = new CashFlow(
            "Test Cash Flow",
            100.00m,
            1,
            DateTime.Now.Year,
            _user,
            CashFlowStatus.PENDING,
            CashFlowType.INCOME
        );
    }

    [Fact]
    public void ShouldBeCreated()
    {
        // Arrange
        var description = "Test Cash Flow";
        var amount = 100.00m;
        var mouth = 1;
        var year = DateTime.Now.Year;
        var user = _user;

        // Act
        var cashFlow = new CashFlow(
            description,
            amount,
            mouth,
            year,
            user,
            CashFlowStatus.PENDING,
            CashFlowType.INCOME
        );

        // Assert
        Assert.NotNull(cashFlow);
        Assert.IsType<CashFlow>(cashFlow);
    }

    [Fact]
    public void ShouldHaveUniqueId()
    {
        // Arrange
        var cashFlow2 = new CashFlow(
            "Another Cash Flow",
            200.00m,
            2,
            DateTime.Now.Year,
            _user,
            CashFlowStatus.PENDING,
            CashFlowType.EXPENSE
        );

        // Assert
        Assert.NotEqual(_cashFlow.Id, cashFlow2.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ShouldNotAllowEmptyDescription(string description)
    {
        // Arrange
        var amount = 100.00m;
        var mouth = 1;
        var year = DateTime.Now.Year;
        var user = _user;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new CashFlow(
                description,
                amount,
                mouth,
                year,
                user,
                CashFlowStatus.PENDING,
                CashFlowType.INCOME
            )
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-100.00)]
    public void ShouldThrowForInvalidAmount(decimal amount)
    {
        // Arrange
        var description = "Test Cash Flow";
        var mouth = 1;
        var year = DateTime.Now.Year;
        var user = _user;

        // Act
        CashFlow action()
        {
            return new CashFlow(
                description,
                amount,
                mouth,
                year,
                user,
                CashFlowStatus.PENDING,
                CashFlowType.INCOME
            );
        }

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(13)]
    public void ShouldThrowForInvalidMonth(int mouth)
    {
        // Arrange
        var description = "Test Cash Flow";
        var amount = 100.00m;
        var year = DateTime.Now.Year;
        var user = _user;

        // Act
        CashFlow action()
        {
            return new CashFlow(
                description,
                amount,
                mouth,
                year,
                user,
                CashFlowStatus.PENDING,
                CashFlowType.INCOME
            );
        }

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void ShouldThrowForInvalidYear()
    {
        // Arrange
        var description = "Test Cash Flow";
        var amount = 100.00m;
        var mouth = 1;
        var years = new[] { 0, DateTime.Now.Year - 1 };
        var user = _user;

        foreach (var year in years)
        {
            // Act
            CashFlow action()
            {
                return new CashFlow(
                    description,
                    amount,
                    mouth,
                    year,
                    user,
                    CashFlowStatus.PENDING,
                    CashFlowType.INCOME
                );
            }

            // Assert
            Assert.Throws<ArgumentException>(action);
        }
    }

    [Fact]
    public void ShouldHaveUserAssociation()
    {
        // Assert
        Assert.NotNull(_cashFlow.User);
        Assert.Equal(_user.Id, _cashFlow.UserId);
    }

    [Fact]
    public void ShouldThrowsForNullUser()
    {
        // Arrange
        var description = "Test Cash Flow";
        var amount = 100.00m;
        var mouth = 1;
        var year = DateTime.Now.Year;

        // Act
        CashFlow action()
        {
            return new CashFlow(
                description,
                amount,
                mouth,
                year,
                null!,
                CashFlowStatus.PENDING,
                CashFlowType.INCOME
            );
        }

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
