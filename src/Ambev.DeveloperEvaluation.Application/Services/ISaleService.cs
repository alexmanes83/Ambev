using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.DTOs;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public interface ISaleService
    {
        Task<SaleDto> GetByIdAsync(Guid id);
        Task<SaleDto> GetBySaleNumberAsync(string saleNumber);
        Task<IEnumerable<SaleDto>> GetAllAsync();
        Task<IEnumerable<SaleDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<SaleDto>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<SaleDto>> GetByBranchIdAsync(string branchId);
        Task<SaleDto> CreateAsync(CreateSaleDto createSaleDto);
        Task<SaleDto> UpdateAsync(Guid id, CreateSaleDto updateSaleDto);
        Task DeleteAsync(Guid id);
        Task<SaleDto> CancelAsync(Guid id);
        Task<SaleDto> CancelItemAsync(Guid saleId, string productId);
    }
} 