using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(
         IdentityUser user);
    }
}
