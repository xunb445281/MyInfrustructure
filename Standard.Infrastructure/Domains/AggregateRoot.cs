using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Standard.Infrastructure.Domains
{
    public abstract class AggregateRoot<Tkey> : EntityBase<Tkey>, IAggregateRoot<Tkey>, IEntity<Tkey>, IDomainObject
    {
        [ConcurrencyCheck]
        public int Version
        {
            get;
            set;
        }

        public AggregateRoot(Tkey id)
        {
            base.Id = id;
        }

        public AggregateRoot()
        {
        }
    }

    public abstract class AggregateRoot : AggregateRoot<long>
    {
        public AggregateRoot(long id) : base(id)
        {
        }

        public AggregateRoot()
        {
        }
    }
}
