using Domain.Dtos.CashFlow;
using Domain.Models;

namespace Domain.Test.Dtos;

public class CreateCashFlowTest
{
    [Fact]
    public void ShouldHaveCorrectProperties()
    {
        var dto = new CreateCashFlow
        {
            Description = "Test Cash Flow",
            Amount = 100.00m,
            Status = CashFlowStatus.PENDING,
            Type = CashFlowType.INCOME,
            UserId = "user123",
        };

        Assert.Equal("Test Cash Flow", dto.Description);
        Assert.Equal(100.00m, dto.Amount);
        Assert.Equal(CashFlowStatus.PENDING, dto.Status);
        Assert.Equal(CashFlowType.INCOME, dto.Type);
        Assert.Equal("user123", dto.UserId);
    }

    [Fact]
    public void UserIdShouldBeSettable()
    {
        var dto = new CreateCashFlow
        {
            Description = "Test",
            Amount = 100m,
            Status = CashFlowStatus.PENDING,
            Type = CashFlowType.INCOME,
            UserId = "newUserId",
        };

        Assert.Equal("newUserId", dto.UserId);
    }
}
