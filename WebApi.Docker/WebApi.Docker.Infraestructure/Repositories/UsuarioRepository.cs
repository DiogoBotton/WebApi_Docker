using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Docker.Domain.AggregateModels.UsuarioAggregate;
using WebApi.Docker.Domain.AggregateModels.UsuarioAggregate.Models;
using WebApi.Docker.Domain.SeedWork;
using WebApi.Docker.Infraestructure.Contexts;

namespace WebApi.Docker.Infraestructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public DockerSqlContext _ctx { get; set; }
        public IUnitOfWork UnitOfWork => _ctx;

        public UsuarioRepository(DockerSqlContext ctx)
        {
            _ctx = ctx;
        }

        public Usuario Create(Usuario objeto)
        {
            return _ctx.Usuarios.Add(objeto).Entity;
        }

        public Usuario GetByEmail(string email)
        {
            return _ctx.Usuarios.FirstOrDefault(x => x.Email == email);
        }

        public List<Usuario> ListAllUsuarios()
        {
            return _ctx.Usuarios.ToList();
        }

        public Usuario UpdateUsuario(Usuario usuario)
        {
            return _ctx.Usuarios.Update(usuario).Entity;
        }

        public Usuario DeleteUsuario(Usuario usuario)
        {
            return _ctx.Remove(usuario).Entity;
        }
    }
}
