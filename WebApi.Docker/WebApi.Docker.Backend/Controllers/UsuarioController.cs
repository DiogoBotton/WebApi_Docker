using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Docker.Backend.Domain.AggregateModels.UsuarioAggregate;
using WebApi.Docker.Backend.Domain.AggregateModels.UsuarioAggregate.Models;
using WebApi.Docker.Backend.DTOs.Inputs;

namespace WebApi.DockerDB.SqlServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public IUsuarioRepository _usuarioRepository { get; set; }

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario(UsuarioInput input)
        {
            try
            {
                Usuario newUsuario = new Usuario(input.Nome, input.Email, input.Senha);

                _usuarioRepository.Create(newUsuario);

                // Salva as alterações no banco
                await _usuarioRepository.UnitOfWork.SaveDbChanges();

                return StatusCode(200, $"Usuario {input.Email} criado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu algum erro na criação do usuário");
            }
        }

        [HttpGet("all-users")]
        public IActionResult GetAllUsuarios()
        {
            try
            {
                var usuarios = _usuarioRepository.ListAllUsuarios();

                // Utilizando tipos anônimos para retornar lista de usuarios modificada (sem a senha)
                var usuariosViewModel = usuarios.Select(x => new
                {
                    x.Id,
                    x.Nome,
                    x.Email
                }).ToList();

                return StatusCode(200, usuariosViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu algum erro no retorno de todos os usuarios.");
            }
        }
    }
}
