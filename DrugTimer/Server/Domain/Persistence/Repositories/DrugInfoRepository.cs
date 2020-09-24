using DrugTimer.Server.Domain.Persistence.Contexts;
using DrugTimer.Server.Domain.Repositories;
using DrugTimer.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Server.Domain.Persistence.Repositories
{
    public class DrugInfoRepository : BaseRepository, IDrugInfoRepistory
    {
        public DrugInfoRepository(AppDbContext context) : base (context) { }

        public async Task<IEnumerable<DrugInfo>> ListAsync()
        {
            return await _context.drugInfos.ToListAsync();
        }
    }
}
