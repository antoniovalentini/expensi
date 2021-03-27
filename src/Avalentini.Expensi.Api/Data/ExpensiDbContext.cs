using Avalentini.Expensi.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Avalentini.Expensi.Api.Data
{
    public class ExpensiDbContext : DbContext
    {
        public ExpensiDbContext(DbContextOptions options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ExpenseMongoEntity> Expenses { get; set; }
    }
}
