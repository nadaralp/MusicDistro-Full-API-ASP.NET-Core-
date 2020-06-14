using MusicDistro.Core;
using MusicDistro.Core.Entities;
using MusicDistro.Core.Events.UserActionAudit;
using MusicDistro.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistro.Services
{
    class MusicService : IMusicService
    {
        private readonly IUnitOfWork _unitOfWork;


        public MusicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Music> CreateMusic(Music newMusic, bool returnWithRelations = false)
        {
            await _unitOfWork.Musics.AddAsync(newMusic);
            await _unitOfWork.CommitAsync();

            if (returnWithRelations)
            {
                return await _unitOfWork.Musics.GetWithArtistByIdAsync(newMusic.Id);
            }

            return newMusic;
        }

        public async Task<Music> DeleteMusic(Music music)
        {
            _unitOfWork.Musics.Remove(music);
            await _unitOfWork.CommitAsync();

            return music;
        }

        public async Task<IEnumerable<Music>> GetAllWithArtist()
        {
            return await _unitOfWork.Musics.GetAllWithArtistAsync();

        }

        public async Task<Music> GetMusicById(int id)
        {
            return await _unitOfWork.Musics.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Music>> GetMusicsByArtistId(int id)
        {
            return await _unitOfWork.Musics.GetAllWithArtistByArtistId(id);
        }

        public async Task<Music> GetMusicWithArtistById(int id)
        {
            return await _unitOfWork.Musics.GetWithArtistByIdAsync(id);
        }

        public async Task<Music> UpdateMusic(Music musicToBeUpdated, Music newMusic)
        {
            _unitOfWork.Musics.Update(musicToBeUpdated, newMusic);
            await _unitOfWork.CommitAsync();

            return newMusic;
        }
    }
}
