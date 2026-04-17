using Domain.Dtos.CashFlow;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class CashFlowService(FinanceiroContext context)
{
    public async Task<IEnumerable<CashFlow>> GetAllAsync(CashFlowsGetAllDto query)
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
            queryable = queryable.Where(c => c.Month == query.Month);
        }

        if (query.Year > 0)
        {
            queryable = queryable.Where(c => c.Year == query.Year);
        }

        if (query.Status.HasValue)
        {
            queryable = queryable.Where(c => c.Status == query.Status.Value);
        }

        if (query.Type is not null)
        {
            queryable = queryable.Where(c => c.Type == query.Type);
        }

        queryable = queryable.Where(c => c.UserId == query.UserId);

        return await queryable.AsNoTracking().ToListAsync();
    }

    public async Task<CashFlow> AddAsync(CreateCashFlowDto dto, string userId)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var cashFlow = new CashFlow(
            dto.Description,
            dto.Amount,
            dto.Month,
            dto.Year,
            user,
            dto.Status,
            dto.Type
        );

        context.CashFlows.Add(cashFlow);
        await context.SaveChangesAsync();

        return await context.CashFlows.AsNoTracking().FirstOrDefaultAsync(c => c.Id == cashFlow.Id);
    }

    public async Task<CashFlow> GetByIdAsync(string id, string userId)
    {
        return await context
            .CashFlows.AsNoTracking()
            .FirstAsync(c => (c.Id == id || c.Description.Contains(id)) && c.UserId == userId);
    }

    public async Task<CashFlow> UpdateAsync(string id, CreateCashFlowDto updateCashFlow)
    {
        context
            .CashFlows.Where(c => c.Id == id)
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
