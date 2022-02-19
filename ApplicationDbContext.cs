using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreOverride
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TestEvent> TestEvents { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<FileUploadEvent>().HasBaseType<TestEvent>();
        }
    }
}
