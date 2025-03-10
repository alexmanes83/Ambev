using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        private List<DomainEvent> _domainEvents;
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
            _domainEvents = new List<DomainEvent>();
        }

        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
} 