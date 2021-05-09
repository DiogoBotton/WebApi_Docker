using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Docker.Backend.Domain.SeedWork
{
    public class AbstractDomain
    {
        public virtual long Id { get; protected set; }
        
    }
}
