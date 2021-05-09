using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Docker.Domain.AggregateModels.UsuarioAggregate.Models;
using WebApi.Docker.Domain.SeedWork;
using WebApi.Docker.Infraestructure.EntityTypeConfigurations;

namespace WebApi.Docker.Infraestructure.Contexts
{
    public class DockerSqlContext : DbContext, IUnitOfWork
    {
        public DbSet<Usuario> Usuarios { get; set; }

        public DockerSqlContext(DbContextOptions<DockerSqlContext> options) : base(options)
        {
        }

        // Método necessário para fazer a criação/mapeamento dos modelos (Domains) no DB
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Classes para mapear as entidades do banco
            modelBuilder.ApplyConfiguration(new UsuarioEntityTypeConfiguration());

            //Caso houver uma referencia de 2 Foreign Key's para uma mesma tabela, o loop altera o comportamento de Cascata para Restrito
            //[...] automaticamente ao fazer a migration
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }
        }

        //Padrão de repositorios usando UnitOfWork, salva alterações na DB independentemente de quais tabelas foram alteradas.
        public async Task SaveDbChanges(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync();
        }
    }

}
