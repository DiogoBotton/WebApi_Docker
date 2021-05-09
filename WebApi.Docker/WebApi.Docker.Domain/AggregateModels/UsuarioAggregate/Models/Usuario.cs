using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Docker.Domain.SeedWork;

namespace WebApi.Docker.Domain.AggregateModels.UsuarioAggregate.Models
{
    public class Usuario : AbstractDomain
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public Usuario(string nome, string email, string senha)
        {
            this.Nome = nome;
            this.Email = email;
            this.Senha = senha;
        }
    }
}
