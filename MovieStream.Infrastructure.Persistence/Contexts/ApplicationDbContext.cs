using Microsoft.EntityFrameworkCore;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TvSerie> TvSeries { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<ProductionCompany> ProductionCompanies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region tables
            modelBuilder.Entity<Movie>().ToTable("Movies");
            modelBuilder.Entity<TvSerie>().ToTable("TvSeries");
            modelBuilder.Entity<Season>().ToTable("Seasons");
            modelBuilder.Entity<Episode>().ToTable("Episodes");
            modelBuilder.Entity<Genre>().ToTable("Genres");
            modelBuilder.Entity<ProductionCompany>().ToTable("ProductionCompanies");
            #endregion

            #region "primary keys"
            modelBuilder.Entity<TvSerie>().HasKey(tvSerie => tvSerie.Id);
            modelBuilder.Entity<Movie>().HasKey(movie => movie.Id);
            modelBuilder.Entity<Season>().HasKey(season => season.Id);
            modelBuilder.Entity<Episode>().HasKey(episode => episode.Id);
            modelBuilder.Entity<Genre>().HasKey(genre => genre.Id);
            modelBuilder.Entity<ProductionCompany>().HasKey(company => company.Id);
            #endregion

            #region relationships

            #region "relationships for genres"
            modelBuilder.Entity<TvSerie>()
                .HasMany(tvSerie => tvSerie.Genres)
                .WithMany(genre => genre.TvSeries)
                .UsingEntity(j => j.ToTable("TvSeriesGenre"));

            modelBuilder.Entity<Movie>()
                .HasMany(movie => movie.Genres)
                .WithMany(genre => genre.Movies)
                .UsingEntity(j => j.ToTable("MovieGenres"));
            #endregion

            #region "relationships for seasons and episodes"
            modelBuilder.Entity<TvSerie>()
                .HasMany(tvSerie => tvSerie.Seasons)
                .WithOne(season => season.TvSerie)
                .HasForeignKey(season => season.TvSerieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Season>()
                .HasMany(season => season.Episodes)
                .WithOne(episode => episode.Season)
                .HasForeignKey(episode => episode.SeasonId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region "relationships for production companies"
            modelBuilder.Entity<ProductionCompany>()
                .HasMany(company => company.TvSeries)
                .WithOne(tvSerie => tvSerie.ProductionCompany)
                .HasForeignKey(tvSerie => tvSerie.ProductionCompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductionCompany>()
                .HasMany(company => company.Movies)
                .WithOne(movie => movie.ProductionCompany)
                .HasForeignKey(movie => movie.ProductionCompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #endregion

            #region "property configuration"

            #region "tv serie"
            modelBuilder.Entity<TvSerie>()
                .Property(ts => ts.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<TvSerie>()
                .Property(ts => ts.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<TvSerie>()
                .Property(ts => ts.ImagePath)
                .IsRequired()
                .IsUnicode(false);

            modelBuilder.Entity<TvSerie>()
                .Property(ts => ts.ReleaseDate)
                .IsRequired();
            #endregion

            #region movie
            modelBuilder.Entity<Movie>()
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Movie>()
                .Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Movie>()
                .Property(m => m.ImagePath)
                .IsRequired()
                .IsUnicode(false);

            modelBuilder.Entity<Movie>()
                .Property(m => m.Runtime)
                .IsRequired();

            modelBuilder.Entity<Movie>()
                .Property(m => m.ReleaseDate)
                .IsRequired();
            #endregion

            #region episode
            modelBuilder.Entity<Episode>()
                .Property(e => e.EpisodeNumber)
                .IsRequired();

            modelBuilder.Entity<Episode>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Episode>()
                .Property(e => e.Description)
                .HasMaxLength(300);

            modelBuilder.Entity<Episode>()
                .Property(e => e.Runtime)
                .IsRequired();

            modelBuilder.Entity<Episode>()
                .Property(e => e.ReleaseDate)
                .IsRequired();

            modelBuilder.Entity<Episode>()
                .Property(e => e.StreamingUrl)
                .IsRequired()
                .IsUnicode(false);
            #endregion

            #region season
            modelBuilder.Entity<Season>()
                .Property(e => e.SeasonNumber)
                .IsRequired();

            modelBuilder.Entity<Season>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Season>()
                .Property(e => e.Description)
                .HasMaxLength(400);

            modelBuilder.Entity<Season>()
                .Property(e => e.ReleaseDate)
                .IsRequired();
            #endregion

            #region genre
            modelBuilder.Entity<Genre>()
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);
            #endregion

            #region "production company"
            modelBuilder.Entity<ProductionCompany>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            #endregion

            #endregion
        }
    }
}
