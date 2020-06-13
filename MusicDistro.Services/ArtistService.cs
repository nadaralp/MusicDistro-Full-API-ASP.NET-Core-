using MusicDistro.Core;
using MusicDistro.Core.Entities;
using MusicDistro.Core.Repositories;
using MusicDistro.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistro.Services
{
    class ArtistService : IArtistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _artistRepository = unitOfWork.Artists;
        }

        public async Task<Artist> CreateArtist(Artist newArtist)
        {
            await _artistRepository.AddAsync(newArtist);
            await _unitOfWork.CommitAsync();

            return newArtist;
        }

        public async Task<Artist> DeleteArtist(Artist artist)
        {
            _artistRepository.Remove(artist);
            await _unitOfWork.CommitAsync();

            return artist;
        }

        public async Task<IEnumerable<Artist>> GetAllArtists()
        {
            return await _artistRepository.GetAllAsync();
        }

        public async Task<Artist> GetArtistById(int id)
        {
            return await _artistRepository.GetByIdAsync(id);
        }

        public async Task<Artist> UpdateArtist(Artist artistToBeUpdated, Artist newArtist)
        {
            _artistRepository.Update(artistToBeUpdated, newArtist);
            await _unitOfWork.CommitAsync();

            return newArtist;
        }
    }
}
