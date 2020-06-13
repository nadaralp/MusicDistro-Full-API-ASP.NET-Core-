using AutoMapper;
using MusicDistro.Api.DTO.Read;
using MusicDistro.Api.DTO.Write;
using MusicDistro.Core.Entities;
using MusicDistro.Core.Entities.Auth;

namespace MusicDistro.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Resource
            CreateMap<Music, MusicDtoR>();
            CreateMap<Artist, ArtistDtoR>();

            // Resource to Domain
            CreateMap<MusicDtoW, Music>();
            CreateMap<UserSignUp, User>();
        }
    }
}
