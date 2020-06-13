using MusicDistro.Api.Utils.Hateoas;

namespace MusicDistro.Api.DTO.Read
{
    public class MusicDtoR : HateoasDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ArtistDtoR Artist { get; set; }
    }
}
