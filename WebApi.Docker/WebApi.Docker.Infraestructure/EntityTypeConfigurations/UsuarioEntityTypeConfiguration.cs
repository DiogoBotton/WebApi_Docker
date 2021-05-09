using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Docker.Domain.AggregateModels.UsuarioAggregate.Models;

namespace WebApi.Docker.Infraestructure.EntityTypeConfigurations
{
    public class UsuarioEntityTypeConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Senha).IsRequired();
        }
    }
}
