using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class LoginDto
    {
        public required string username { get; set; }

        public required string password  { get; set; }
    }
}