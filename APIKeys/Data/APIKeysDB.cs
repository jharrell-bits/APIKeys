using APIKeys.Models;
using Microsoft.EntityFrameworkCore;

namespace APIKeys.Data
{
    public class APIKeysDB : DbContext
    {
        public APIKeysDB(DbContextOptions<APIKeysDB> options)
            : base(options) { }

        public DbSet<APIKey> APIKeys => Set<APIKey>();
    }
}