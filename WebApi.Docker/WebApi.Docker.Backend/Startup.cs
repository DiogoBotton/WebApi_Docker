using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Docker.Backend.Domain.AggregateModels.UsuarioAggregate;
using WebApi.Docker.Backend.Helpers;
using WebApi.Docker.Backend.Infraestructure.Contexts;
using WebApi.Docker.Backend.Infraestructure.Repositories;

namespace WebApi.Docker.Backend
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Vinculando Interfaces de repositório com o repositório em si para o mesmo ser acessado apenas a partir da interface
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // Configurações do DB
            services.AddDbContext<DockerSqlContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    // Este código "MigrationsAssembly" é necessário para fazer o comando "add-migration", para criar os arquivos necessários e realizar o comando seguinte "update-database" (Code First)
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Configuração para criar as tabelas na DB automaticamente quando iniciar o aplicativo (Injeção de Dependência)
            app.PrepararDb();
            //PrepareDB.PrepararDb(app);

            app.UseMvc();
        }
    }
}
