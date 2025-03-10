using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.DTOs
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public bool IsCancelled { get; set; }
        public decimal Total { get; set; }
        public IEnumerable<SaleItemDto> Items { get; set; }
    }

    public class SaleItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public bool IsCancelled { get; set; }
        public decimal Total { get; set; }
    }
} 