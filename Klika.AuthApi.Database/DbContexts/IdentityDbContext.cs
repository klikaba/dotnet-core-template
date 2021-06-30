using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Klika.AuthApi.Model.Entities;

namespace Klika.AuthApi.Database.DbContexts
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Ctor
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options) { }
        #endregion
    }
}
