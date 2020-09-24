using DrugTimer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Server.Domain.Repositories
{
    public interface IDrugInfoRepistory
    {
        Task<IEnumerable<DrugInfo>> ListAsync();
    }
}
