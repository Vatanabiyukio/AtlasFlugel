# AtlasFlugel API

Bem-vindo ao projeto AtlasFlugel API! Este repositório contém uma aplicação .NET Core que oferece uma API para gerenciar pedidos.

## Funcionalidades

- CRUD completo para pedidos (GET, POST, PUT e DELETE).

![dbo](https://github.com/Vatanabiyukio/AtlasFlugel/assets/56417398/92f34d14-e491-416d-8b21-c692de8916ce)

### Modelo de JSON para Pedido

```JSON
{
  "id": 1,
  "nomeCliente": "Nome do Cliente",
  "emailCliente": "cliente@email.com",
  "pago": true,
  "valorTotal": 50.0,
  "itensPedido": [
    {
      "id": 1,
      "idProduto": 1,
      "nomeProduto": "Produto A",
      "valorUnitario": 25.0,
      "quantidade": 2
    }
  ]
}
```

## Configuração

### Pré-requisitos

- [.NET SDK 6.0](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)

### Instruções de Execução

1. Clone o repositório:

   ```bash
   git clone https://github.com/seu-usuario/atlasflugel-api.git

2. Navegue até o diretório do projeto:

    ```bash
    cd atlasflugel-api

3. Execute o seguinte comando para iniciar a aplicação:

    ```bash
    docker-compose up -d

4. Aguarde a construção e a execução dos contêineres.

5. Acesse a documentação da API Swagger:

http://localhost:80/swagger

### Observação

Certifique-se de que a porta 80 não está em uso por outras aplicações em seu ambiente.
