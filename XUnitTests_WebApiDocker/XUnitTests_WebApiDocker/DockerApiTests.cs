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

        // IMPORTANTE: Os testes, por padrão, serão executados em ordem alfabética, caso haja necessidade de ordenar os testes, certificar que os NOMES dos métodos correspondem a ordem desejada

        // Fact é usado para definir que o método abaixo será de testes
        [Fact(DisplayName = "Criação de usuário (POST)")]
        public async void A_Post_CreateUsuario_Return_OK()
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
        [Theory(DisplayName = "Retornando um usuário existente por email (GET)")]
        // Enviar parametros via atributo InlineData
        [InlineData("admin@email.com")]
        public async void C_Get_GetUsuarioByEmail_Return_Usuario(string email)
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
        public async void D_Get_GetUsuarioByEmail_Return_NotFound(string email)
        {
            // Arrange
            Uri uri = new Uri($"http://localhost:8000/api/Usuario/email/{email}");

            // Act
            var response = await _client.GetAsync(uri);

            // Assert (Verifica se o StatusCode retornado corresponse ao status "Found", de código 302)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // Este é a única "falha" da aplicação, mas ele falha pois os testes não são feitos de forma ordenada
        // Neste caso, 2 testes são feitos com esse usuário específico, Update e Delete, mas o Delete de alguma forma ocorre antes do Update (mesmo com o método delete sendo os últimos métodos da classe)
        // Solução: Ordenar os testes a partir dos nomes dos métodos
        [Fact(DisplayName = "Alteração de usuário")]
        public async void E_Post_AlterUsuario_Return_OK()
        {
            // Arrange

            // Cria Uri com o id do usuario (para não depender de outros métodos, será adicionado diretamente o ID do usuário no BD)
            Uri uriAlter = new Uri("http://localhost:8000/api/Usuario/alter-user/2");

            // Edita usuário
            var Usuario = new
            {
                Nome = "Diogo" + " EDITADO :)",
                Email = "diogo@email.com",
                Senha = "diogo1234"
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
        public async void F_Post_CreateUsuario_Return_Forbidden()
        {
            // Arrange

            // Cria Uri com o id do usuario admin padrão (id 1)
            Uri uriAlter = new Uri("http://localhost:8000/api/Usuario/alter-user/1");

            // Edita usuário
            var Usuario = new
            {
                Nome = "Admin" + " EDITADO :)",
                Email = "naovaidanao@email.com",
                Senha = "admin1234"
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
        public async void G_Post_DeleteUsuario_Return_OK()
        {
            // Arrange

            // Cria Uri com o id do usuario que será excluido (id 2)
            Uri uriDel = new Uri("http://localhost:8000/api/Usuario/delete/2");

            // Act
            HttpResponseMessage responseAlter = await _client.DeleteAsync(uriDel);

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseAlter.StatusCode);
        }

        [Fact(DisplayName = "Deletar usuário proibido")]
        public async void H_Post_DeleteUsuario_Return_Forbidden()
        {
            // Arrange

            // Cria Uri com o id do usuario que não poderá ser excluido (id 1, admin)
            Uri uriDel = new Uri("http://localhost:8000/api/Usuario/delete/1");

            // Act
            HttpResponseMessage responseDel = await _client.DeleteAsync(uriDel);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, responseDel.StatusCode);
        }
    }
}