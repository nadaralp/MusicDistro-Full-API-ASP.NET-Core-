using MusicDistro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistro.Core.Repositories
{
    public interface IUserAuditRepository : IRepository<UserAudit>
    {
        Task Commit();
    }
}
