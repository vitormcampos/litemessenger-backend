using System.Security.Claims;
using Application.Services;
using Domain.Dtos.CashFlow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CashFlowController : ControllerBase
{
    private readonly CashFlowService _cashflowService;

    public CashFlowController(CashFlowService cashflowService)
    {
        _cashflowService = cashflowService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateCashFlow cashflow)
    {
        var userId = User.FindFirstValue(ClaimTypes.Sid);
        cashflow.UserId = userId;

        var result = await _cashflowService.AddAsync(cashflow);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CashFlowsGetAll query)
    {
        var userId = User.FindFirstValue(ClaimTypes.Sid);

        var result = await _cashflowService.GetAllAsync(query with { UserId = userId });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.Sid);

        var result = await _cashflowService.GetByIdAsync(id, userId);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CreateCashFlow cashflow)
    {
        var userId = User.FindFirstValue(ClaimTypes.Sid);
        cashflow.UserId = userId;

        var result = await _cashflowService.UpdateAsync(id, cashflow);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.Sid);

        await _cashflowService.DeleteAsync(id, userId);

        return NoContent();
    }
}
