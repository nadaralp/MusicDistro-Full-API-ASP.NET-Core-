using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicDistro.Core.Entities;

namespace MusicDistro.Core.Services
{
    public interface IArtistService
    {
        Task<IEnumerable<Artist>> GetAllArtists();
        Task<Artist> GetArtistById(int id);
        Task<Artist> CreateArtist(Artist newArtist);
        Task<Artist> UpdateArtist(Artist artistToBeUpdated, Artist newArtist);
        Task<Artist> DeleteArtist(Artist artist);
        
    }
}
