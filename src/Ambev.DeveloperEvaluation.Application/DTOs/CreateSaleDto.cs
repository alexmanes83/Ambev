using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.DTOs
{
    public class CreateSaleDto
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public IEnumerable<CreateSaleItemDto> Items { get; set; }
    }

    public class CreateSaleItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
} 