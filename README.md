# WebApi_Docker
Aprendendo a desenvolver uma API utilizando containers do Docker. API .NET Core e Banco de Dados Sql Server.

## Instalar o Docker Desktop para Windows e executando uma instância SQL Server do Docker:
https://docs.docker.com/docker-for-windows/install/

**Após baixado e instalado**
* Abra o PowerShell
* Cole e execute este comando para baixar imagem estável do SQL Server para Docker

### `docker pull mcr.microsoft.com/mssql/server:2019-latest`

* Após o download, já será possível criar uma instância utilizando essa imagem com o comando abaixo

### `docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=SenhaDoSA' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest`

* Substitua "SenhaDoSA" para alguma senha forte de 8 caracteres, com letras maiusculas e minusculas, números e caracteres especiais (Exigência do próprio SQL Server)

## Executar API e Banco de Dados em um container do Docker

**Com este projeto de exemplo foi preciso realizar:**
* Criar API com Suporte ao Docker, com isto será criado o arquivo "Dockerfile" (Caso não colocou, basta clicar com botão direito no seu projeto >> Adicionar >> Suporte do Docker)
* Criar todo o tipo de lógica necessária de entidades, repositórios, controllers, etc
* Arquivo "appsettings.json" configurado com a String de Conexão do Banco de dados que será criado com o docker-compose
* Após realizado todo o código referente às entidades, criação de migration com o comando "add-migration [nome]"
* Injeção de Dependência "PrepareDb" na StartUp para criar as tabelas e adicionar dados nas mesmas caso estejam vazias automaticamente quando iniciado a API
* Criação de arquivo "docker-compose.yml" para fazer a criação de API + Banco de dados em um só container com apenas um comando (há comentarios nesse arquivo para apoio)
* Build e Up da aplicação via CLI com docker-compose

## Para executar esta aplicação no seu pc será preciso:
* Ter o Docker Desktop para Windows instalado no seu pc
* Abrir o projeto no Visual Studio, abrir o Console de Gerenciador de Pacotes e digitar o comando abaixo caso não exista a pasta "Migrations":

### `add-migration NomeDaMigration`

* Após criado a migration, poderá fechar o VS, abra o projeto com o console do windows ou Git Bash dentro da pasta do projeto "WebApi.Docker" e digite:

### `docker-compose build`

* Este comando fará o build da sua aplicação. **Sempre que alterar o código, deverá fazer uma nova build**
* Após isso, poderá subir a aplicação localmente no Docker com o comando:

### `docker-compose up -d`

* Este comendo baixará todas as dependências se for preciso e subirá a API + Banco de Dados no Docker, como configurado no arquivo docker-compose.yml

**Para testar:**
* Criar um usuario: http://localhost:8000/api/Usuario (POST)
* Retornar todos os usuários: http://localhost:8000/api/Usuario/all-users (GET)

## Referências:

* [DockerHub Microsoft SQL Server](https://hub.docker.com/_/microsoft-mssql-server)
* [Docker Compose with .NET Core & SQL Server (Step by Step)](https://www.youtube.com/watch?v=4V7CwC_4oss)
* [Como criar o seu ambiente de estudos em SQL Server usando Docker!](https://www.youtube.com/watch?v=qNQPWdHkPNw)