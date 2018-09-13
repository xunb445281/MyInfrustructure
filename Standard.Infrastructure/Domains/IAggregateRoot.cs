using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Domains
{
    public interface IAggregateRoot<out TKey> : IEntity<TKey>, IDomainObject
    {

    }
}
