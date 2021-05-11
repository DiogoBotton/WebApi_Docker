using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;
using XUnitTests_WebApiDocker.ViewModels;

namespace XUnitTests_WebApiDocker
{
    public class DockerApiTests
    {
        public HttpClient _client { get; set; }

        public DockerApiTests()
        {
            _client = new HttpClient();
        }

        // Fact é usado para definir que o método abaixo será de testes
        [Fact(DisplayName = "Criação de usuário (POST)")]
        public async void Post_CreateUsuario_Return_OK()
        {
            // Arrange -> É onde está a preparação para fazer o teste (declaração de variaveis, objetos, etc)
            Uri uri = new Uri("http://localhost:8000/api/Usuario");
            var Usuario = new
            {
                Nome = "Fulano de Tal",
                Email = "fulano@email.com",
                Senha = "fulano123"
            };

            // Transforma objeto usuario em JSON (string)
            string data = JsonConvert.SerializeObject(Usuario);
            // Vincula data ao conteudo string do tipo json que será enviado à API
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            // Act (Action) -> É onde a ação de teste ocorre
            HttpResponseMessage response = await _client.PostAsync(uri, content);

            // Assert -> Verifica se o resultado da ação é o esperado pelo teste (neste caso, se a resposta http é igual à OK = 200)
            // Casting para converter de Enum para Int
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact(DisplayName = "Retornar todos os usuarios (GET)")]
        public async void Get_GetAllUsuarios_Return_Usuarios()
        {
            // Arrange
            Uri uri = new Uri("http://localhost:8000/api/Usuario/all-users");

            // Act 
            var response = await _client.GetAsync(requestUri: uri);

            // Assert
            Assert.Equal(200, (int)response.StatusCode);
        }

        // Theory utilizado para testes parametrizados
        [Theory(DisplayName = "Retornando um usuário existente por email (GET)")]
        // Enviar parametros via atributo InlineData
        [InlineData("diogo@email.com")]
        public async void Get_GetUsuarioByEmail_Return_Usuario(string email)
        {
            // Arrange
            Uri uri = new Uri($"http://localhost:8000/api/Usuario/email/{email}");

            // Act
            var response = await _client.GetAsync(uri);

            // Assert (Verifica se o StatusCode retornado corresponse ao status "Found", de código 302)
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
        }

        [Theory(DisplayName = "Retornando um usuário NÃO existente por email (GET)")]
        [InlineData("nãoexiste@email.com")]
        public async void Get_GetUsuarioByEmail_Return_NotFound(string email)
        {
            // Arrange
            Uri uri = new Uri($"http://localhost:8000/api/Usuario/email/{email}");

            // Act
            var response = await _client.GetAsync(uri);

            // Assert (Verifica se o StatusCode retornado corresponse ao status "Found", de código 302)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "Alteração de usuário")]
        public async void Post_AlterUsuario_Return_OK()
        {
            // Arrange
            Uri uriGet = new Uri("http://localhost:8000/api/Usuario/email/fulano@email.com");

            // Busca usuário por email que será editado na API
            string responseGet = await _client.GetStringAsync(uriGet);

            // Desserializa o objeto JSON transformando em um ViewModel específico
            var usuario = JsonConvert.DeserializeObject<UsuarioViewModel>(responseGet);

            // Cria Uri com o id do usuario retornado
            Uri uriAlter = new Uri($"http://localhost:8000/api/Usuario/alter-user/{usuario.Id}");

            // Edita usuário
            var Usuario = new
            {
                Nome = usuario.Nome + " EDITADO :)",
                Email = usuario.Email,
                Senha = "fulano1234"
            };

            // Transforma objeto usuario em JSON (string)
            string data = JsonConvert.SerializeObject(Usuario);
            // Vincula data ao conteudo string do tipo json que será enviado à API
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage responseAlter = await _client.PutAsync(uriAlter, content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseAlter.StatusCode);
        }

        [Fact(DisplayName = "Alteração de usuário proibido")]
        public async void Post_CreateUsuario_Return_Forbidden()
        {
            // Arrange
            Uri uriGet = new Uri("http://localhost:8000/api/Usuario/email/diogo@email.com");

            // Busca usuário por email que será editado na API
            string responseGet = await _client.GetStringAsync(uriGet);

            // Desserializa o objeto JSON transformando em um ViewModel específico
            var usuario = JsonConvert.DeserializeObject<UsuarioViewModel>(responseGet);

            // Cria Uri com o id do usuario retornado
            Uri uriAlter = new Uri($"http://localhost:8000/api/Usuario/alter-user/{usuario.Id}");

            // Edita usuário
            var Usuario = new
            {
                Nome = usuario.Nome + " EDITADO :)",
                Email = usuario.Email,
                Senha = "diogo1234"
            };

            // Transforma objeto usuario em JSON (string)
            string data = JsonConvert.SerializeObject(Usuario);
            // Vincula data ao conteudo string do tipo json que será enviado à API
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage responseAlter = await _client.PutAsync(uriAlter, content);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, responseAlter.StatusCode);
        }

        [Fact(DisplayName = "Deletar usuário")]
        public async void Post_DeleteUsuario_Return_OK()
        {
            // Arrange
            Uri uriGet = new Uri("http://localhost:8000/api/Usuario/email/fulano@email.com");

            // Busca usuário por email que será excluido na API
            string responseGet = await _client.GetStringAsync(uriGet);

            // Desserializa o objeto JSON transformando em um ViewModel específico
            var usuario = JsonConvert.DeserializeObject<UsuarioViewModel>(responseGet);

            // Cria Uri com o id do usuario retornado
            Uri uriDel = new Uri($"http://localhost:8000/api/Usuario/delete/{usuario.Id}");

            // Act
            HttpResponseMessage responseAlter = await _client.DeleteAsync(uriDel);

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseAlter.StatusCode);
        }

        [Fact(DisplayName = "Deletar usuário proibido")]
        public async void Post_DeleteUsuario_Return_Forbidden()
        {
            // Arrange
            Uri uriGet = new Uri("http://localhost:8000/api/Usuario/email/diogo@email.com");

            // Busca usuário por email que será excluido na API
            string responseGet = await _client.GetStringAsync(uriGet);

            // Desserializa o objeto JSON transformando em um ViewModel específico
            var usuario = JsonConvert.DeserializeObject<UsuarioViewModel>(responseGet);

            // Cria Uri com o id do usuario retornado
            Uri uriDel = new Uri($"http://localhost:8000/api/Usuario/delete/{usuario.Id}");

            // Act
            HttpResponseMessage responseDel = await _client.DeleteAsync(uriDel);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, responseDel.StatusCode);
        }
    }
}