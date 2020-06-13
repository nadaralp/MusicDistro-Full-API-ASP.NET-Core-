using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicDistro.Api.DTO.Write
{
    public class UserSignUp
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}
