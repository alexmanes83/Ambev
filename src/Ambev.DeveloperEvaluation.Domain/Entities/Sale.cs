using System;
using System.Collections.Generic;
using System.Linq;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : Entity
    {
        public string SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }
        public string CustomerId { get; private set; }
        public string CustomerName { get; private set; }
        public string BranchId { get; private set; }
        public string BranchName { get; private set; }
        public bool IsCancelled { get; private set; }
        private readonly List<SaleItem> _items;
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        public Sale(string saleNumber, string customerId, string customerName, string branchId, string branchName)
        {
            SaleNumber = saleNumber;
            SaleDate = DateTime.UtcNow;
            CustomerId = customerId;
            CustomerName = customerName;
            BranchId = branchId;
            BranchName = branchName;
            IsCancelled = false;
            _items = new List<SaleItem>();
        }

        public void AddItem(string productId, string productName, decimal unitPrice, int quantity)
        {
            if (IsCancelled)
                throw new DomainException("Não é possivel adicionar item para uma venda cancelada!");

            if (quantity > 20)
                throw new DomainException("Não é possível vender acima de 20 itens idênticos!");

            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(quantity);
            }
            else
            {
                _items.Add(new SaleItem(productId, productName, unitPrice, quantity));
            }

            AddDomainEvent(new SaleModifiedEvent(Id));
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new DomainException("Venda já foi cancelada");

            IsCancelled = true;
            AddDomainEvent(new SaleCancelledEvent(Id));
        }

        public void CancelItem(string productId)
        {
            if (IsCancelled)
                throw new DomainException("Não é possível cancelar itens de uma venda cancelada!");

            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new DomainException("Item não encontrado!");

            item.Cancel();
            AddDomainEvent(new ItemCancelledEvent(Id, productId));
        }

        public decimal CalculateTotal()
        {
            return _items.Where(i => !i.IsCancelled).Sum(i => i.CalculateTotal());
        }

        public decimal CalculateItemTotal(string productId)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            return item?.CalculateTotal() ?? 0;
        }
    }
} 