using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SqlVisitsRepository
{
    public partial class VisitsContext : DbContext
    {
        public virtual DbSet<UserVisits> UserVisits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=tcp:restapiprojectspiveyserver.database.windows.net,1433;Initial Catalog=restapiprojectspivey;Persist Security Info=False;User ID=puthere;Password=puthere;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserVisits>(entity =>
            {
                entity.HasKey(e => e.VisitId)
                    .HasName("PK__UserVisi__4D3AA1DE35B880BD");

                entity.Property(e => e.VisitId).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("datetime");
            });
        }
    }
}