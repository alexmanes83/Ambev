using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetAll()
        {
            var sales = await _saleService.GetAllAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleDto>> GetById(Guid id)
        {
            var sale = await _saleService.GetByIdAsync(id);
            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        [HttpGet("number/{saleNumber}")]
        public async Task<ActionResult<SaleDto>> GetBySaleNumber(string saleNumber)
        {
            var sale = await _saleService.GetBySaleNumberAsync(saleNumber);
            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var sales = await _saleService.GetByDateRangeAsync(startDate, endDate);
            return Ok(sales);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetByCustomerId(string customerId)
        {
            var sales = await _saleService.GetByCustomerIdAsync(customerId);
            return Ok(sales);
        }

        [HttpGet("branch/{branchId}")]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetByBranchId(string branchId)
        {
            var sales = await _saleService.GetByBranchIdAsync(branchId);
            return Ok(sales);
        }

        [HttpPost]
        public async Task<ActionResult<SaleDto>> Create(CreateSaleDto createSaleDto)
        {
            var sale = await _saleService.CreateAsync(createSaleDto);
            return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SaleDto>> Update(Guid id, CreateSaleDto updateSaleDto)
        {
            try
            {
                var sale = await _saleService.UpdateAsync(id, updateSaleDto);
                return Ok(sale);
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _saleService.DeleteAsync(id);
                return NoContent();
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<SaleDto>> Cancel(Guid id)
        {
            try
            {
                var sale = await _saleService.CancelAsync(id);
                return Ok(sale);
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{saleId}/items/{productId}/cancel")]
        public async Task<ActionResult<SaleDto>> CancelItem(Guid saleId, string productId)
        {
            try
            {
                var sale = await _saleService.CancelItemAsync(saleId, productId);
                return Ok(sale);
            }
            catch (Domain.Exceptions.DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 