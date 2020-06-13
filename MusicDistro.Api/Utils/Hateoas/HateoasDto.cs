using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicDistro.Api.Utils.Hateoas
{
    public abstract class HateoasDto
    {
        public IEnumerable<Link> ResourceLinks { get; set; }
    }
}
