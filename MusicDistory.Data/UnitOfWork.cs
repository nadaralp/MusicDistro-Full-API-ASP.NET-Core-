using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicDistory.Data.Repositories;
using MusicDistro.Core;
using MusicDistro.Core.Repositories;

namespace MusicDistory.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MusicDbContext _context;
        private MusicRepository _musicRepository;
        private ArtistRepository _artistRepository;

        public UnitOfWork(MusicDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IMusicRepository Musics =>
            _musicRepository ??= new MusicRepository(_context);


        public IArtistRepository Artists
            => _artistRepository ??= new ArtistRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
