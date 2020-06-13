using MusicDistro.Core.Entities.Auth;
using System;
using System.Collections.Generic;

namespace MusicDistro.Core.Services
{
    public interface IJwtService
    {
        string GenerateToken(string secret, string issuer, double expirationInDays, string audience, User user, IList<string> roles);
    }
}
