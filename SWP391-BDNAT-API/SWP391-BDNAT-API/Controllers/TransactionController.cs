﻿using BDNAT_Repository.DTO;
using BDNAT_Repository.Implementation;
using BDNAT_Service.Implementation;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionDTO>>> GetAllTransactions()
        {
            try
            {
                var list = await _transactionService.GetAllTransactionsAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDTO>> GetTransactionById(int id)
        {
            try
            {
                var item = await _transactionService.GetTransactionByIdAsync(id);
                if (item == null)
                    return NotFound($"Transaction with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{UserId}/getByUserId")]
        public async Task<ActionResult<List<BookingDisplayDTO>>> GettTransactionByUserId(int UserId)
        {
            try
            {
                var item = await _transactionService.GetTransactionByUserIdAsync(UserId);
                if (item == null)
                    return NotFound($"Booking with ID {UserId} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateTransaction([FromBody] TransactionDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _transactionService.CreateTransactionAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateTransaction([FromBody] TransactionDTO dto)
        {
            try
            {
                var result = await _transactionService.UpdateTransactionAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteTransaction(int id)
        {
            try
            {
                var result = await _transactionService.DeleteTransactionAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("receive-hook")]
        public async Task<IActionResult> ReceiveHook([FromBody] JsonElement payload)
        {
            var success = await _transactionService.HandleWebhookAsync(payload);
            if (success)
            {
                return Ok("Transaction updated successfully");
            }
            else
            {
                return StatusCode(500, "Failed to handle webhook");
            }
        }
    }
}
