using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Docker.Domain.SeedWork
{
    public interface IRepository<T> where T : AbstractDomain
    {
        IUnitOfWork UnitOfWork { get; }
        T Create(T objeto);
    }
}
