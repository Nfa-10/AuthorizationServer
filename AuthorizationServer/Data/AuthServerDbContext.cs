using Microsoft.EntityFrameworkCore;

namespace AuthorizationServer.Data
{
    public class AuthServerDbContext:DbContext
    {
        public AuthServerDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
