using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Domains
{
    public interface IEntity<out TKey> : IDomainObject
    {
        TKey Id
        {
            get;
        }
    }
}
