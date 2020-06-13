using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicDistro.Core.Entities;

namespace MusicDistro.Core.Repositories
{
    public interface IArtistRepository : IRepository<Artist>
    {
        Task<IEnumerable<Artist>> GetAllWithMusicAsync();
    }
}
