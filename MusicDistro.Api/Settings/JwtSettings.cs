using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicDistro.Api.Settings
{
    /// <summary>
    /// Class to access options with IOptionsSnapshot (OptionsPattern)
    /// </summary>
    public class JwtSettings
    {
        public const string Jwt = "Jwt";

        public string Issuer { get; set; }
        public string Secret { get; set; }
        public string ExpirationInDays { get; set; }
    }
}
