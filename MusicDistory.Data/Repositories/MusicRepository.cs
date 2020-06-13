using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicDistro.Core.Entities;
using MusicDistro.Core.Repositories;
using TDistory.Data.Repositories;

namespace MusicDistory.Data.Repositories
{
    public class MusicRepository : Repository<Music>, IMusicRepository
    {

        private MusicDbContext _context => Context as MusicDbContext;

        public MusicRepository(MusicDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Music>> GetAllWithArtistAsync()
        {
            return
                await _context.Musics.Include(a => a.Artist).ToListAsync();
        }

        public async Task<Music> GetWithArtistByIdAsync(int id)
        {
            return
                await _context
                      .Musics
                      .Include(m => m.Artist).
                      FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Music>> GetAllWithArtistByArtistId(int id)
        {
            return
                await _context
                    .Musics
                    .Include(m => m.Artist)
                    .Where(m => m.ArtistId == id)
                    .ToListAsync();
        }
    }
}
