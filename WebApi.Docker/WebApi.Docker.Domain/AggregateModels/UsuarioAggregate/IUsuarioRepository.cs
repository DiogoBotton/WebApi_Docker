using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Docker.Domain.AggregateModels.UsuarioAggregate.Models;
using WebApi.Docker.Domain.SeedWork;

namespace WebApi.Docker.Domain.AggregateModels.UsuarioAggregate
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario GetByEmail(string email);
        List<Usuario> ListAllUsuarios();
        Usuario UpdateUsuario(Usuario usuario);
        Usuario DeleteUsuario(Usuario usuario);
    }
}
