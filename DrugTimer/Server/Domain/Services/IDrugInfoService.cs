using DrugTimer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Server.Domain.Services
{
    public interface IDrugInfoService
    {
        Task<IEnumerable<DrugInfo>> ListAsync();
    }
}
