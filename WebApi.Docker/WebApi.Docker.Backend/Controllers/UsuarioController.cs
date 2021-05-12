using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                var usuarioDb = _usuarioRepository.GetByEmail(input.Email);

                // Validação para verificar se já existe usuário com email igual já cadastrado
                if (usuarioDb != null)
                    return StatusCode(403, $"Já existe um usuário com o email {input.Email}.");

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

        [HttpGet("email/{email}")]
        public IActionResult GetUsuarioByEmail(string email)
        {
            try
            {
                var usuario = _usuarioRepository.GetByEmail(email);

                // 404
                if (usuario == null)
                    return StatusCode((int)HttpStatusCode.NotFound, "Usuário não encontrado");

                // Utilizando tipos anônimos para retornar objeto usuario (sem a senha)
                var usuarioViewModel = new
                {
                    usuario.Id,
                    usuario.Nome,
                    usuario.Email
                };

                // 302
                return StatusCode((int)HttpStatusCode.Found, usuarioViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu algum erro no retorno do usuário específicado.");
            }
        }

        [HttpPut("alter-user/{id}")]
        public async Task<IActionResult> PutUsuario(UsuarioInput input, int id)
        {
            try
            {
                var usuarioDb = _usuarioRepository.GetById(id);

                if (usuarioDb == null)
                    return StatusCode((int)HttpStatusCode.NotFound, "Este usuário não existe para ser alterado.");

                // Caso o usuário seja o que foi criado por padrão na inicialização da API
                if (usuarioDb.Email == "admin@email.com")
                    return StatusCode((int)HttpStatusCode.Forbidden, "Você não pode alterar o usuário admin padrão.");

                usuarioDb.AlterarDados(input.Nome, input.Email, input.Senha);

                _usuarioRepository.UpdateUsuario(usuarioDb);

                // Salva na DB
                await _usuarioRepository.UnitOfWork.SaveDbChanges();

                return StatusCode(200, $"Usuário id [{usuarioDb.Id}] editado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu algum erro na alteração do usuário específicado.");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var usuarioDb = _usuarioRepository.GetById(id);

                // Caso usuário não exista
                if (usuarioDb == null)
                    return StatusCode((int)HttpStatusCode.NotFound, "Este usuário não existe.");

                // Caso o usuário seja o que foi criado por padrão na inicialização da API
                if (usuarioDb.Email == "admin@email.com")
                    return StatusCode((int)HttpStatusCode.Forbidden, "Você não pode excluir o usuário admin padrão.");

                _usuarioRepository.DeleteUsuario(usuarioDb);

                // Salva na DB
                await _usuarioRepository.UnitOfWork.SaveDbChanges();

                return StatusCode((int) HttpStatusCode.OK, "Usuário excluído com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu algum erro ao deletar o usuário específicado.");
            }
        }
    }
}
