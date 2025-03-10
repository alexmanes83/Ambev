using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public abstract class DomainEvent
    {
        public DateTime Timestamp { get; }

        protected DomainEvent()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
} 