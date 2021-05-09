using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Docker.Backend.Domain.AggregateModels.UsuarioAggregate.Models;
using WebApi.Docker.Backend.Domain.SeedWork;

namespace WebApi.Docker.Backend.Domain.AggregateModels.UsuarioAggregate
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario GetByEmail(string email);
        List<Usuario> ListAllUsuarios();
        Usuario UpdateUsuario(Usuario usuario);
        Usuario DeleteUsuario(Usuario usuario);
    }
}
