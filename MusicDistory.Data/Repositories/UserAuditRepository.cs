using Microsoft.EntityFrameworkCore;
using MusicDistro.Core.Entities;
using MusicDistro.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDistory.Data.Repositories;

namespace MusicDistory.Data.Repositories
{
    public class UserAuditRepository : Repository<UserAudit>, IUserAuditRepository
    {


        public UserAuditRepository(MusicDbContext dbContext) : base(dbContext)
        {
        }

        public async Task Commit()
        {
            await Context.SaveChangesAsync();
        }
    }
}
