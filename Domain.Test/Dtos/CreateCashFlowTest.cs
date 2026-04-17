using Domain.Dtos.CashFlow;
using Domain.Models;

namespace Domain.Test.Dtos;

public class CreateCashFlowTest
{
    [Fact]
    public void ShouldHaveCorrectProperties()
    {
        var dto = new CreateCashFlowDto
        {
            Description = "Test Cash Flow",
            Amount = 100.00m,
            Status = CashFlowStatus.PENDING,
            Type = CashFlowType.INCOME,
        };

        Assert.Equal("Test Cash Flow", dto.Description);
        Assert.Equal(100.00m, dto.Amount);
        Assert.Equal(CashFlowStatus.PENDING, dto.Status);
        Assert.Equal(CashFlowType.INCOME, dto.Type);
    }
}
