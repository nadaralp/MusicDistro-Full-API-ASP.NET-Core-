using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDistro.Core.Entities.Auth
{
    /// <summary>
    /// for the Role class I’m not going to add any new properties, I just created it so it is cleaner when using this class.
    /// </summary>
    public class Role : IdentityRole<Guid>
    {
    }
}
