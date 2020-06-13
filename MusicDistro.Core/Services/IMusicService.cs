using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicDistro.Core.Entities;

namespace MusicDistro.Core.Services
{
    public interface IMusicService
    {
        Task<IEnumerable<Music>> GetAllWithArtist();
        Task<Music> GetMusicById(int id);
        Task<Music> GetMusicWithArtistById(int id);
        Task<IEnumerable<Music>> GetMusicsByArtistId(int id);
        Task<Music> CreateMusic(Music newMusic, bool returnWithRelations = false);
        Task<Music> UpdateMusic(Music musicToBeUpdated, Music newMusic);
        Task<Music> DeleteMusic(Music music);
    }
}
