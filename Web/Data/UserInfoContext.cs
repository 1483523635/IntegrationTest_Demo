using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Web.Models
{
    public class UserInfoContext : DbContext
    {
        public UserInfoContext (DbContextOptions<UserInfoContext> options)
            : base(options)
        {
        }

        public DbSet<Web.Models.UserInfoDto> UserInfoDto { get; set; }
    }
}
