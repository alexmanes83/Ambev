using System;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : Entity
    {
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public decimal Discount { get; private set; }
        public bool IsCancelled { get; private set; }

        public SaleItem(string productId, string productName, decimal unitPrice, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            UpdateQuantity(quantity);
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity > 20)
                throw new DomainException("Não é possível vender acima de 20 itens idênticos");

            Quantity = quantity;
            CalculateDiscount();
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new DomainException("Item já foi cancelado");

            IsCancelled = true;
        }

        public decimal CalculateTotal()
        {
            if (IsCancelled)
                return 0;

            var subtotal = UnitPrice * Quantity;
            return subtotal - (subtotal * Discount);
        }
        // Calculando desconto de acordo com a regra de negócio definido pelo projeto
        private void CalculateDiscount()
        {
            if (Quantity < 4)
            {
                Discount = 0;
                return;
            }

            if (Quantity >= 10 && Quantity <= 20)
            {
                Discount = 0.20m;
                return;
            }

            if (Quantity >= 4)
            {
                Discount = 0.10m;
                return;
            }

            Discount = 0;
        }
    }
} 