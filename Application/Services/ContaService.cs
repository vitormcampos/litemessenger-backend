using Domain.Dtos.CashFlow;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class CashFlowService(FinanceiroContext context)
{
    public async Task<IEnumerable<CashFlow>> GetAllAsync(CashFlowsGetAll query)
    {
        var queryable = context.CashFlows.AsQueryable();

        if (query.Description is not null)
        {
            queryable = queryable.Where(c => c.Description.Contains(query.Description));
        }

        if (query.MinValue > 0)
        {
            queryable = queryable.Where(c => c.Amount >= query.MinValue);
        }

        if (query.MaxValue > 0)
        {
            queryable = queryable.Where(c => c.Amount <= query.MaxValue);
        }

        if (query.Month > 0)
        {
            queryable = queryable.Where(c => c.Mouth == query.Month);
        }

        if (query.Year > 0)
        {
            queryable = queryable.Where(c => c.Year == query.Year);
        }

        if (!string.IsNullOrEmpty(query.Status))
        {
            queryable = queryable.Where(c => c.Status == query.Status);
        }

        if (!string.IsNullOrEmpty(query.Type))
        {
            queryable = queryable.Where(c => c.Type == query.Type);
        }

        queryable = queryable.Where(c => c.UserId == query.UserId);

        return await queryable.AsNoTracking().ToListAsync();
    }

    public async Task<CashFlow> AddAsync(CreateCashFlow createCashFlow)
    {
        var cashFlow = new CashFlow
        {
            Id = Guid.NewGuid().ToString(),
            Description = createCashFlow.Description,
            Amount = createCashFlow.Amount,
            Status = createCashFlow.Status,
            Type = createCashFlow.Type,
            Mouth = DateTime.UtcNow.Month,
            Year = DateTime.UtcNow.Year,
            CreatedAt = DateTime.UtcNow,
            UserId = createCashFlow.UserId!,
        };

        context.CashFlows.Add(cashFlow);
        await context.SaveChangesAsync();

        return cashFlow;
    }

    public async Task<CashFlow> GetByIdAsync(string id, string userId)
    {
        return await context
            .CashFlows.AsNoTracking()
            .FirstAsync(c => (c.Id == id || c.Description.Contains(id)) && c.UserId == userId);
    }

    public async Task<CashFlow> UpdateAsync(string id, CreateCashFlow updateCashFlow)
    {
        context
            .CashFlows.Where(c => c.Id == id && c.UserId == updateCashFlow.UserId)
            .ExecuteUpdate(c =>
                c.SetProperty(c => c.Description, updateCashFlow.Description)
                    .SetProperty(c => c.Amount, updateCashFlow.Amount)
                    .SetProperty(c => c.Status, updateCashFlow.Status)
            );

        await context.SaveChangesAsync();

        var cashFlow = await context.CashFlows.FirstAsync(c => c.Id == id);
        return cashFlow;
    }

    public async Task DeleteAsync(string id, string userId)
    {
        await context.CashFlows.Where(c => c.Id == id && c.UserId == userId).ExecuteDeleteAsync();
    }
}
