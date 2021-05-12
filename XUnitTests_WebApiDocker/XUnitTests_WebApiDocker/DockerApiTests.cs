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

        // IMPORTANTE: Os testes, por padr�o, ser�o executados em ordem alfab�tica, caso haja necessidade de ordenar os testes, certificar que os NOMES dos m�todos correspondem a ordem desejada

        // Fact � usado para definir que o m�todo abaixo ser� de testes
        [Fact(DisplayName = "Cria��o de usu�rio (POST)")]
        public async void A_Post_CreateUsuario_Return_OK()
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
        public async void B_Get_GetAllUsuarios_Return_Usuarios()
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
        [InlineData("admin@email.com")]
        public async void C_Get_GetUsuarioByEmail_Return_Usuario(string email)
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
        public async void D_Get_GetUsuarioByEmail_Return_NotFound(string email)
        {
            // Arrange
            Uri uri = new Uri($"http://localhost:8000/api/Usuario/email/{email}");

            // Act
            var response = await _client.GetAsync(uri);

            // Assert (Verifica se o StatusCode retornado corresponse ao status "Found", de c�digo 302)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // Este � a �nica "falha" da aplica��o, mas ele falha pois os testes n�o s�o feitos de forma ordenada
        // Neste caso, 2 testes s�o feitos com esse usu�rio espec�fico, Update e Delete, mas o Delete de alguma forma ocorre antes do Update (mesmo com o m�todo delete sendo os �ltimos m�todos da classe)
        // Solu��o: Ordenar os testes a partir dos nomes dos m�todos
        [Fact(DisplayName = "Altera��o de usu�rio")]
        public async void E_Post_AlterUsuario_Return_OK()
        {
            // Arrange

            // Cria Uri com o id do usuario (para n�o depender de outros m�todos, ser� adicionado diretamente o ID do usu�rio no BD)
            Uri uriAlter = new Uri("http://localhost:8000/api/Usuario/alter-user/2");

            // Edita usu�rio
            var Usuario = new
            {
                Nome = "Diogo" + " EDITADO :)",
                Email = "diogo@email.com",
                Senha = "diogo1234"
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
        public async void F_Post_CreateUsuario_Return_Forbidden()
        {
            // Arrange

            // Cria Uri com o id do usuario admin padr�o (id 1)
            Uri uriAlter = new Uri("http://localhost:8000/api/Usuario/alter-user/1");

            // Edita usu�rio
            var Usuario = new
            {
                Nome = "Admin" + " EDITADO :)",
                Email = "naovaidanao@email.com",
                Senha = "admin1234"
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
        public async void G_Post_DeleteUsuario_Return_OK()
        {
            // Arrange

            // Cria Uri com o id do usuario que ser� excluido (id 2)
            Uri uriDel = new Uri("http://localhost:8000/api/Usuario/delete/2");

            // Act
            HttpResponseMessage responseAlter = await _client.DeleteAsync(uriDel);

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseAlter.StatusCode);
        }

        [Fact(DisplayName = "Deletar usu�rio proibido")]
        public async void H_Post_DeleteUsuario_Return_Forbidden()
        {
            // Arrange

            // Cria Uri com o id do usuario que n�o poder� ser excluido (id 1, admin)
            Uri uriDel = new Uri("http://localhost:8000/api/Usuario/delete/1");

            // Act
            HttpResponseMessage responseDel = await _client.DeleteAsync(uriDel);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, responseDel.StatusCode);
        }
    }
}