using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.model.identity;

namespace talabatRepository.Identity
{
   public class addIdentityDbcontext : IdentityDbContext<appUser>
    {
        public addIdentityDbcontext(DbContextOptions<addIdentityDbcontext> options) : base(options)
        {
        }
       
    }
   
}
