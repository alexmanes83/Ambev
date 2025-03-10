using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly IMongoCollection<Sale> _sales;

        public SaleRepository(IMongoDatabase database)
        {
            _sales = database.GetCollection<Sale>("sales");
        }

        public async Task<Sale> GetByIdAsync(Guid id)
        {
            return await _sales.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Sale> GetBySaleNumberAsync(string saleNumber)
        {
            return await _sales.Find(s => s.SaleNumber == saleNumber).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _sales.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _sales.Find(s => s.SaleDate >= startDate && s.SaleDate <= endDate).ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByCustomerIdAsync(string customerId)
        {
            return await _sales.Find(s => s.CustomerId == customerId).ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByBranchIdAsync(string branchId)
        {
            return await _sales.Find(s => s.BranchId == branchId).ToListAsync();
        }

        public async Task AddAsync(Sale sale)
        {
            await _sales.InsertOneAsync(sale);
        }

        public async Task UpdateAsync(Sale sale)
        {
            await _sales.ReplaceOneAsync(s => s.Id == sale.Id, sale);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _sales.DeleteOneAsync(s => s.Id == id);
        }
    }
} 