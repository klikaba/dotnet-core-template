using Klika.ResourceApi.Model.Entities.TemplateDbContext;
using Microsoft.EntityFrameworkCore;

namespace Klika.ResourceApi.Database.DbContexts
{
    public class TemplateDbContext : DbContext
    {
        public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options) { }

        public DbSet<TemplateEntity> TemplateTable { get; set; }

        /// <summary>
        /// Use this method for FluentAPI configuration.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
