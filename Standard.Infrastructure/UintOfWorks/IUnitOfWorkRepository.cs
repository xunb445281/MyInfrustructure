using Standard.Infrastructure.Domains;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Standard.Infrastructure.UintOfWorks
{
    public interface IUnitOfWorkRepository<TAggregateRoot>
    where TAggregateRoot : AggregateRoot
    {
        void Add(TAggregateRoot entity);

        Task AddAsync(TAggregateRoot entity);

        void Delete(TAggregateRoot entity);

        Task DeleteAsync(TAggregateRoot entity);

        Task DeleteAsync(long id);

        void Update(TAggregateRoot entity);

        Task UpdateAsync(TAggregateRoot entity);
    }
}
