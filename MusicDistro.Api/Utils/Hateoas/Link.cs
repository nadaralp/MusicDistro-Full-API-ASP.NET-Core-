using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicDistro.Api.Utils.Hateoas
{
    /// <summary>
    /// HATEOAS convention
    /// Hyper Media as the engine of the application.
    /// This interface exposes a link to the resource
    /// </summary>
    public class Link
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
    }
}
