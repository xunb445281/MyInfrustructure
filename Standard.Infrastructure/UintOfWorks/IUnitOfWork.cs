using Standard.Infrastructure.Domains;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Standard.Infrastructure.UintOfWorks
{
    public interface IUnitOfWork
    {
        void Commint();

        Task CommitAsync();
    }
}
