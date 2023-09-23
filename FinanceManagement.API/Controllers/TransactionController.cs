using FinanceManagement.BLL.Services.Interfaces;
using FinanceManagement.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IFinancialTransactionService _transactionService;

        public TransactionController(IFinancialTransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [Authorize(Policy = "AllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FinancialTransactionDTO>>> GetTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FinancialTransactionDTO>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            return Ok(transaction);
        }

        [HttpPost]
        [Authorize(Policy = "AllUsers")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTransaction(FinancialTransactionDTO transactionDto)
        {
            var transaction = await _transactionService.CreateTransactionAsync(transactionDto);
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AllUsers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTransaction(int id, FinancialTransactionDTO transactionDto)
        {
            if (id != transactionDto.Id)
            {
                return BadRequest();
            }

            await _transactionService.UpdateTransactionAsync(transactionDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AllUsers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            await _transactionService.DeleteTransactionAsync(id);
            return NoContent();
        }
    }
}
