
using Microsoft.EntityFrameworkCore;
using NextMindBackEnd.Models;

namespace NextMindBackEnd.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
    }
}

