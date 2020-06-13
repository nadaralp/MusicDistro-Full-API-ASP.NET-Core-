using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicDistro.Api.DTO.Read;
using MusicDistro.Api.DTO.Write;
using MusicDistro.Api.Utils.Hateoas;
using MusicDistro.Core.Entities;
using MusicDistro.Core.Services;

namespace MusicDistro.Api.Controllers
{
    [Route("api/musics")]
    [Authorize]
    public class MusicController : HateoasBaseController
    {
        private readonly IMusicService _musicService;
        private readonly IMapper _mapper;

        public MusicController(IMusicService musicService, IMapper mapper)
        {
            _musicService = musicService;
            _mapper = mapper;
        }


        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> Musics()
        {
            var musics = await _musicService.GetAllWithArtist();
            if (musics is null)
                return BadRequest();

            var musicsDto = _mapper.Map<IEnumerable<MusicDtoR>>(musics);


            return Ok(musicsDto);

        }

        [HttpGet("{id}", Name = MethodNames.GetMethod)]
        public async Task<ActionResult<MusicDtoR>> GetMusic(int id)
        {
            var music = await _musicService.GetMusicWithArtistById(id);
            if (music is null)
                return NotFound();

            var musicDto = _mapper.Map<MusicDtoR>(music);
            BuildLinksForObject(musicDto, id);

            return musicDto;
        }

        [HttpPost("", Name = MethodNames.PostMethod)]
        public async Task<ActionResult<MusicDtoR>> AddMusic(MusicDtoW musicDto)
        {
            var validator = new MusicDtoW.Validator().Validate(musicDto);
            if (!validator.IsValid)
            {
                return BadRequest(validator.Errors);
            }
            var musicEntity = _mapper.Map<Music>(musicDto);
            var musicCraeted = await _musicService.CreateMusic(musicEntity, true);


            var musicReadDto = _mapper.Map<MusicDtoR>(musicCraeted);
            BuildLinksForObject(musicReadDto, musicReadDto.Id);

            HttpContext.Response.StatusCode = 201;
            return musicReadDto;
        }

        [HttpPut("id", Name = MethodNames.PutMethod)]
        public async Task<IActionResult> UpdateMusic(int id, MusicDtoW musicDto)
        {
            var oldMusic = await _musicService.GetMusicById(id);
            if (oldMusic is null)
            {
                return NotFound();
            }

            var music = _mapper.Map<Music>(musicDto);
            var updatedMusic = await _musicService.UpdateMusic(oldMusic, music);

            var updatedMusicDto = _mapper.Map<MusicDtoR>(updatedMusic);
            BuildLinksForObject(updatedMusicDto, updatedMusic.Id);

            return Ok(updatedMusic);
        }

        [HttpDelete("id", Name = MethodNames.DeleteMethod)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<MusicDtoR>> DeleteMusic(int id)
        {
            var music = await _musicService.GetMusicWithArtistById(id);
            if (music is null)
            {
                return NotFound();
            }

            await _musicService.DeleteMusic(music);

            return _mapper.Map<MusicDtoR>(music);
        }
    }

}