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

        // Fact � usado para definir que o m�todo abaixo ser� de testes
        [Fact(DisplayName = "Cria��o de usu�rio (POST)")]
        public async void Post_CreateUsuario_Return_OK()
        {
            // Arrange -> � onde est� a prepara��o para fazer o teste (declara��o de variaveis, objetos, etc)
            Uri uri = new Uri("http://localhost:8000/api/Usuario");
            var Usuario = new
            {
                Nome = "Fulano de Tal",
                Email = "fulano@email.com",
                Senha = "fulano123"
            };

            // Transforma objeto usuario em JSON (string)
            string data = JsonConvert.SerializeObject(Usuario);
            // Vincula data ao conteudo string do tipo json que ser� enviado � API
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            // Act (Action) -> � onde a a��o de teste ocorre
            HttpResponseMessage response = await _client.PostAsync(uri, content);

            // Assert -> Verifica se o resultado da a��o � o esperado pelo teste (neste caso, se a resposta http � igual � OK = 200)
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
        [Theory(DisplayName = "Retornando um usu�rio existente por email (GET)")]
        // Enviar parametros via atributo InlineData
        [InlineData("diogo@email.com")]
        public async void Get_GetUsuarioByEmail_Return_Usuario(string email)
        {
            // Arrange
            Uri uri = new Uri($"http://localhost:8000/api/Usuario/email/{email}");

            // Act
            var response = await _client.GetAsync(uri);

            // Assert (Verifica se o StatusCode retornado corresponse ao status "Found", de c�digo 302)
            Assert.Equal(HttpStatusCode.Found, response.StatusCode);
        }

        [Theory(DisplayName = "Retornando um usu�rio N�O existente por email (GET)")]
        [InlineData("n�oexiste@email.com")]
        public async void Get_GetUsuarioByEmail_Return_NotFound(string email)
        {
            // Arrange
            Uri uri = new Uri($"http://localhost:8000/api/Usuario/email/{email}");

            // Act
            var response = await _client.GetAsync(uri);

            // Assert (Verifica se o StatusCode retornado corresponse ao status "Found", de c�digo 302)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "Altera��o de usu�rio")]
        public async void Post_AlterUsuario_Return_OK()
        {
            // Arrange
            Uri uriGet = new Uri("http://localhost:8000/api/Usuario/email/fulano@email.com");

            // Busca usu�rio por email que ser� editado na API
            string responseGet = await _client.GetStringAsync(uriGet);

            // Desserializa o objeto JSON transformando em um ViewModel espec�fico
            var usuario = JsonConvert.DeserializeObject<UsuarioViewModel>(responseGet);

            // Cria Uri com o id do usuario retornado
            Uri uriAlter = new Uri($"http://localhost:8000/api/Usuario/alter-user/{usuario.Id}");

            // Edita usu�rio
            var Usuario = new
            {
                Nome = usuario.Nome + " EDITADO :)",
                Email = usuario.Email,
                Senha = "fulano1234"
            };

            // Transforma objeto usuario em JSON (string)
            string data = JsonConvert.SerializeObject(Usuario);
            // Vincula data ao conteudo string do tipo json que ser� enviado � API
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage responseAlter = await _client.PutAsync(uriAlter, content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseAlter.StatusCode);
        }

        [Fact(DisplayName = "Altera��o de usu�rio proibido")]
        public async void Post_CreateUsuario_Return_Forbidden()
        {
            // Arrange
            Uri uriGet = new Uri("http://localhost:8000/api/Usuario/email/diogo@email.com");

            // Busca usu�rio por email que ser� editado na API
            string responseGet = await _client.GetStringAsync(uriGet);

            // Desserializa o objeto JSON transformando em um ViewModel espec�fico
            var usuario = JsonConvert.DeserializeObject<UsuarioViewModel>(responseGet);

            // Cria Uri com o id do usuario retornado
            Uri uriAlter = new Uri($"http://localhost:8000/api/Usuario/alter-user/{usuario.Id}");

            // Edita usu�rio
            var Usuario = new
            {
                Nome = usuario.Nome + " EDITADO :)",
                Email = usuario.Email,
                Senha = "diogo1234"
            };

            // Transforma objeto usuario em JSON (string)
            string data = JsonConvert.SerializeObject(Usuario);
            // Vincula data ao conteudo string do tipo json que ser� enviado � API
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage responseAlter = await _client.PutAsync(uriAlter, content);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, responseAlter.StatusCode);
        }

        [Fact(DisplayName = "Deletar usu�rio")]
        public async void Post_DeleteUsuario_Return_OK()
        {
            // Arrange
            Uri uriGet = new Uri("http://localhost:8000/api/Usuario/email/fulano@email.com");

            // Busca usu�rio por email que ser� excluido na API
            string responseGet = await _client.GetStringAsync(uriGet);

            // Desserializa o objeto JSON transformando em um ViewModel espec�fico
            var usuario = JsonConvert.DeserializeObject<UsuarioViewModel>(responseGet);

            // Cria Uri com o id do usuario retornado
            Uri uriDel = new Uri($"http://localhost:8000/api/Usuario/delete/{usuario.Id}");

            // Act
            HttpResponseMessage responseAlter = await _client.DeleteAsync(uriDel);

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseAlter.StatusCode);
        }

        [Fact(DisplayName = "Deletar usu�rio proibido")]
        public async void Post_DeleteUsuario_Return_Forbidden()
        {
            // Arrange
            Uri uriGet = new Uri("http://localhost:8000/api/Usuario/email/diogo@email.com");

            // Busca usu�rio por email que ser� excluido na API
            string responseGet = await _client.GetStringAsync(uriGet);

            // Desserializa o objeto JSON transformando em um ViewModel espec�fico
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