﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDistro.Core.Entities
{
    public class Music
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
    }
}
