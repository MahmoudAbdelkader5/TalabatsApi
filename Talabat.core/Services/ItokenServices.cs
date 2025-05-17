using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.model.identity;

namespace Talabat.core.Services
{
    public interface ItokenServices
    {
        Task<string> CreateToken(appUser username);
    }
}
