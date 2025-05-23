using Microsoft.EntityFrameworkCore;
using MotoApi.Models;

namespace MotoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Moto> Motos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moto>(entity =>
            {
                //Este é o owner em que as tabelas foram criadas, meu usuário
                entity.ToTable("MOTO", schema: "RM554889");

                entity.HasKey(e => e.IdMoto);

                entity.Property(e => e.IdMoto)
                      .HasColumnName("ID_MOTO");

                entity.Property(e => e.Modelo)
                      .HasColumnName("MODELO")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.Placa)
                      .HasColumnName("PLACA")
                      .HasMaxLength(7)
                      .IsRequired();

                entity.Property(e => e.StatusMotoId)
                      .HasColumnName("STATUSMOTO_ID_STATUS");
            });
        }

    }
}
