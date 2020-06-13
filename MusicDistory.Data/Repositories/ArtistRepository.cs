using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicDistro.Core.Entities;
using MusicDistro.Core.Repositories;
using TDistory.Data.Repositories;

namespace MusicDistory.Data.Repositories
{
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        private MusicDbContext _context => Context as MusicDbContext;
        public ArtistRepository(MusicDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Artist>> GetAllWithMusicAsync()
        {
            return 
                await _context
                      .Artists
                      .Include(a => a.Musics)
                      .ToListAsync();
        }
    }
}
