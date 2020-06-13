using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDistro.Core.Entities
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Music> Musics { get; set; }
    }
}
