using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Docker.Backend.Domain.AggregateModels.UsuarioAggregate.Models;

namespace WebApi.Docker.Backend.Infraestructure.Contexts
{
    public static class PrepareDB
    {
        // Deixando como paramêtro de "PrepararDb" a interface "IApplicationBuilder", será vísivel na startUp para utiliza-lo
        public static void PrepararDb(this IApplicationBuilder app)
        {
            // Injeção de dependência
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                EnviarDados(serviceScope.ServiceProvider.GetService<DockerSqlContext>());
            }
        }

        public static void EnviarDados(DockerSqlContext ctx)
        {
            // Migração (cria tabelas)
            // OBS. Para funcionar, antes deve se criar a migration com "add-migration [nome]"
            ctx.Database.Migrate();

            // Caso não houver nenhum usuário cadastrado, cadastra um por padrão
            if (!ctx.Usuarios.Any())
            {
                ctx.Usuarios.Add(new Usuario("Diogo", "diogo@email.com", "diogo123"));
                ctx.SaveChanges();
            }
        }
    }
}