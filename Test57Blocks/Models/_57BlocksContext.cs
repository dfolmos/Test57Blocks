using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Test57Blocks.Models
{
    public partial class _57BlocksContext : DbContext
    {
        public _57BlocksContext()
        {
        }

        public _57BlocksContext(DbContextOptions<_57BlocksContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.IdGenre);

                entity.Property(e => e.GenreName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.IdMovie);

                entity.Property(e => e.CreationDate).HasColumnType("smalldatetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("smalldatetime");

                entity.Property(e => e.MovieTitle)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.IdGenreNavigation)
                    .WithMany(p => p.Movies)
                    .HasForeignKey(d => d.IdGenre)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Movies_Genres");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.MovieIdUserNavigations)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Movies_Users");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.MovieModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Movies_Users1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.HasIndex(e => e.UserEmail, "NonClusteredIndex-20211216-223441");

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
