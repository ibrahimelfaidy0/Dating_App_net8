using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;

namespace api.Interfaces
{
    public interface ItokenService
    {
        string CreateToken (AppUser appUser);
    }
}