using FinanceManagement.BLL.Services.Interfaces;
using FinanceManagement.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpGet]
        [Authorize(Policy = "AllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BudgetDTO>>> GetBudgets()
        {
            var budgets = await _budgetService.GetAllBudgetsAsync();
            return Ok(budgets);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BudgetDTO>> GetBudget(int id)
        {
            var budget = await _budgetService.GetBudgetByIdAsync(id);
            return Ok(budget);
        }

        [HttpPost]
        [Authorize(Policy = "AllUsers")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBudget(BudgetDTO budgetDto)
        {
            var budget = await _budgetService.CreateBudgetAsync(budgetDto);
            return CreatedAtAction(nameof(GetBudget), new { id = budget.Id }, budget);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AllUsers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBudget(int id, BudgetDTO budgetDto)
        {
            if (id != budgetDto.Id)
            {
                return BadRequest();
            }

            await _budgetService.UpdateBudgetAsync(budgetDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AllUsers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            await _budgetService.DeleteBudgetAsync(id);
            return NoContent();
        }
    }
}
