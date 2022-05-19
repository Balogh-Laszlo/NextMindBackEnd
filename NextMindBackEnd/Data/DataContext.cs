
using Microsoft.EntityFrameworkCore;
using NextMindBackEnd.Data.Models;
using NextMindBackEnd.Models;

namespace NextMindBackEnd.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<IftttKey> IftttKeys { get; set; }
        public DbSet<Control> Controls { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<RemoteController> RemoteControllers { get; set; }
        public DbSet<PageControl> PageControls { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PageControl>()
                    .HasKey(pc => new { pc.ControlID, pc.PageID });
            modelBuilder.Entity<PageControl>()
                    .HasOne(p => p.Page)
                    .WithMany(pc => pc.PageControls)
                    .HasForeignKey(pc => pc.PageID);
            modelBuilder.Entity<PageControl>()
                .HasOne(p => p.Control)
                .WithMany(pc => pc.PageControls)
                .HasForeignKey(pc => pc.ControlID);

        } 
    }
}

