using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicDistro.Core.Entities;

namespace MusicDistro.Core.Repositories
{
    public interface IMusicRepository : IRepository<Music>
    {
        Task<IEnumerable<Music>> GetAllWithArtistAsync();
        Task<Music> GetWithArtistByIdAsync(int id);
        Task<IEnumerable<Music>> GetAllWithArtistByArtistId(int id);
    }
}
