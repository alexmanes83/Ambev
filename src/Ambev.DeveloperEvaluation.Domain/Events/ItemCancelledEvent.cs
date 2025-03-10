using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class ItemCancelledEvent : DomainEvent
    {
        public Guid SaleId { get; }
        public string ProductId { get; }

        public ItemCancelledEvent(Guid saleId, string productId)
        {
            SaleId = saleId;
            ProductId = productId;
        }
    }
} 