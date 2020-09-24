using DrugTimer.Server.Domain.Repositories;
using DrugTimer.Server.Domain.Services;
using DrugTimer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Server.Services
{
    public class DrugInfoService : IDrugInfoService
    {
        private readonly IDrugInfoRepistory _drugInfoRepistory;

        public DrugInfoService(IDrugInfoRepistory drugInfoRepistory)
        {
            _drugInfoRepistory = drugInfoRepistory;
        }

        public async Task<IEnumerable<DrugInfo>> ListAsync()
        {
            return await _drugInfoRepistory.ListAsync();
        }
    }
}
