using System;
using System.Threading.Tasks;
using MusicDistro.Core.Repositories;

namespace MusicDistro.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IMusicRepository Musics { get; }
        IArtistRepository Artists { get;  }
        Task<int> CommitAsync();
    }
}
