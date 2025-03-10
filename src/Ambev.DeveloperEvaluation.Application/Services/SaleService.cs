using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;

        public SaleService(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<SaleDto> GetByIdAsync(Guid id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            return sale == null ? null : MapToDto(sale);
        }

        public async Task<SaleDto> GetBySaleNumberAsync(string saleNumber)
        {
            var sale = await _saleRepository.GetBySaleNumberAsync(saleNumber);
            return sale == null ? null : MapToDto(sale);
        }

        public async Task<IEnumerable<SaleDto>> GetAllAsync()
        {
            var sales = await _saleRepository.GetAllAsync();
            return sales.Select(MapToDto);
        }

        public async Task<IEnumerable<SaleDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sales = await _saleRepository.GetByDateRangeAsync(startDate, endDate);
            return sales.Select(MapToDto);
        }

        public async Task<IEnumerable<SaleDto>> GetByCustomerIdAsync(string customerId)
        {
            var sales = await _saleRepository.GetByCustomerIdAsync(customerId);
            return sales.Select(MapToDto);
        }

        public async Task<IEnumerable<SaleDto>> GetByBranchIdAsync(string branchId)
        {
            var sales = await _saleRepository.GetByBranchIdAsync(branchId);
            return sales.Select(MapToDto);
        }

        public async Task<SaleDto> CreateAsync(CreateSaleDto createSaleDto)
        {
            var saleNumber = await GenerateSaleNumberAsync();
            var sale = new Sale(saleNumber, createSaleDto.CustomerId, createSaleDto.CustomerName, 
                createSaleDto.BranchId, createSaleDto.BranchName);

            foreach (var item in createSaleDto.Items)
            {
                sale.AddItem(item.ProductId, item.ProductName, item.UnitPrice, item.Quantity);
            }

            await _saleRepository.AddAsync(sale);
            return MapToDto(sale);
        }

        public async Task<SaleDto> UpdateAsync(Guid id, CreateSaleDto updateSaleDto)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
                throw new DomainException("Venda não encontrada");

            if (sale.IsCancelled)
                throw new DomainException("não é possível alterar uma venda cancelada");

            // Clear existing items
            foreach (var item in sale.Items.ToList())
            {
                sale.CancelItem(item.ProductId);
            }

            // Add new items
            foreach (var item in updateSaleDto.Items)
            {
                sale.AddItem(item.ProductId, item.ProductName, item.UnitPrice, item.Quantity);
            }

            await _saleRepository.UpdateAsync(sale);
            return MapToDto(sale);
        }

        public async Task DeleteAsync(Guid id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
                throw new DomainException("Venda não encontrada");

            await _saleRepository.DeleteAsync(id);
        }

        public async Task<SaleDto> CancelAsync(Guid id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
                throw new DomainException("Venda não encontrada");

            sale.Cancel();
            await _saleRepository.UpdateAsync(sale);
            return MapToDto(sale);
        }

        public async Task<SaleDto> CancelItemAsync(Guid saleId, string productId)
        {
            var sale = await _saleRepository.GetByIdAsync(saleId);
            if (sale == null)
                throw new DomainException("Venda não encontrada");

            sale.CancelItem(productId);
            await _saleRepository.UpdateAsync(sale);
            return MapToDto(sale);
        }

        private async Task<string> GenerateSaleNumberAsync()
        {
            // In a real application, this would be more sophisticated
            return $"SALE-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        private static SaleDto MapToDto(Sale sale)
        {
            return new SaleDto
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                SaleDate = sale.SaleDate,
                CustomerId = sale.CustomerId,
                CustomerName = sale.CustomerName,
                BranchId = sale.BranchId,
                BranchName = sale.BranchName,
                IsCancelled = sale.IsCancelled,
                Total = sale.CalculateTotal(),
                Items = sale.Items.Select(item => new SaleItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Discount = item.Discount,
                    IsCancelled = item.IsCancelled,
                    Total = item.CalculateTotal()
                })
            };
        }
    }
} 