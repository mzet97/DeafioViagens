# DesafioViagens

## Arquitetura

O desafio foi desenvolvido utilizando uma arquitetura em 3 camadas, promovendo a separação de responsabilidades e facilitando a manutenção e evolução do sistema.

### Camadas:
1. **Application**:
   - Responsável pela interação com o usuário e exposição das funcionalidades do sistema via **API RESTful** ou **aplicação console**.
   - Utiliza **ASP.NET Core** na API, com suporte a Swagger para documentação interativa.

2. **Domain**:
   - Contém a lógica de negócio central, incluindo o **algoritmo de Dijkstra** para encontrar o menor custo em um grafo.
   - Classes principais:
     - **Airport**: Representa aeroportos, encapsulando informações como o código IATA.
     - **Route**: Representa conexões entre aeroportos, armazenando origem, destino e custo da rota.

3. **Infrastructure**:
   - Gerencia o acesso a dados, inicialmente com arquivos CSV.
   - Preparada para migração futura para um banco de dados relacional, utilizando **Entity Framework Core**.

## O Problema

O problema foi tratado como um grafo direcionado, onde:
- Cada **aeroporto** é representado por um nó.
- Cada **rota** é representada por uma aresta com peso igual ao custo da rota.
- A busca pela rota de menor custo entre dois aeroportos foi realizada com o **algoritmo de Dijkstra**, eficiente para grafos densos e ponderados.

### Fluxo de Dados
1. O sistema carrega o grafo a partir de um arquivo CSV ou banco de dados.
2. Os dados são processados na camada de domínio para construir as instâncias de `Airport` e `Route`.
3. A API expõe métodos para:
   - Consultar o menor caminho e custo entre dois aeroportos.
   - Criar, atualizar e deletar rotas no grafo.

## Melhorias Implementadas e Planejadas

### Implementadas:
- **Algoritmo de Dijkstra** para otimização de custo.
- API com suporte ao padrão RESTful, documentada via Swagger.
- Ferramentas para execução local em diferentes formatos (console e API).

### Planejadas:
1. **Banco de Dados**:
   - Adotar **SQL Server** ou **PostgreSQL** com tabelas normalizadas para armazenar aeroportos e rotas.
   - Criar índices para otimizar a consulta de rotas.

2. **Segurança**:
   - Implementar **OAuth 2.0** ou **JWT** na API.
   - Configurar políticas de CORS para restringir acessos externos.

3. **Escalabilidade e Resiliência**:
   - Adicionar suporte a **cache distribuído** (ex: Redis) para rotas frequentemente acessadas.
   - Implementar **balanceamento de carga** utilizando **NGINX** ou **Azure Load Balancer**.

4. **Testes Automatizados**:
   - Cobrir cenários complexos com testes de carga e integração.
   - Simular casos extremos, como milhões de rotas ou cenários com custos variáveis.

5. **Logs e Monitoramento**:
   - Integrar **Serilog** para logging estruturado.
   - Configurar **Application Insights** ou **Prometheus/Grafana** para monitoramento em produção.

6. **CI/CD**:
   - Utilizar **Azure DevOps** ou **GitHub Actions** para pipelines de integração e entrega contínua.

## Como Executar

### Console

1. Gere o executável executando o comando abaixo no diretório:
   `DesafioViagens\src\DesafioViagens`
   
   ```cmd
   dotnet publish DesafioViagens.Console\DesafioViagens.Console.csproj -c Release -o "path"
   ```

2. Mova o arquivo `rotas.csv` para a pasta do `path`.

3. Execute o arquivo `DesafioViagens.Console.exe` passando o caminho do arquivo CSV como argumento. Exemplo:
   
   ```cmd
   DesafioViagens.Console.exe rotas.csv
   ```

4. Insira a rota e pressione **Enter**, ou apenas pressione **Enter** para sair.

### API

1. Gere o executável executando o comando abaixo no diretório:
   `DesafioViagens\src\DesafioViagens`
   
   ```cmd
   dotnet publish DesafioViagens.API\DesafioViagens.API.csproj -c Release -o "path"
   ```

2. Mova o arquivo `rotas.csv` para a pasta do `path`.

3. Execute o arquivo `DesafioViagens.API.exe` passando o caminho do arquivo CSV como argumento. Exemplo:
   
   ```cmd
   DesafioViagens.API.exe rotas.csv
   ```

4. Acesse o Swagger para testar a API através da URL:
   
   ```
   http://localhost:5000/swagger
   ```

5. **Testando Métodos da API**:
   - **Obter a melhor rota**:
     - Endpoint: `GET /api/v1/routes/best`
     - Parâmetros de consulta:
       - `origin`: Código do aeroporto de origem.
       - `destination`: Código do aeroporto de destino.
     - Exemplo:
       ```
       GET http://localhost:5000/api/v1/routes/best?origin=GRU&destination=JFK
       ```
   
   - **Obter todas as rotas**:
     - Endpoint: `GET /api/v1/routes`
     - Exemplo:
       ```
       GET http://localhost:5000/api/v1/routes
       ```
   
   - **Obter uma rota específica**:
     - Endpoint: `GET /api/v1/routes/{origin}/{destination}`
     - Exemplo:
       ```
       GET http://localhost:5000/api/v1/routes/GRU/JFK
       ```
   
   - **Criar uma nova rota**:
     - Endpoint: `POST /api/v1/routes`
     - Corpo da requisição (JSON):
       ```json
       {
           "origin": "GRU",
           "destination": "JFK",
           "cost": 500
       }
       ```
     - Exemplo:
       ```
       POST http://localhost:5000/api/v1/routes
       ```
   
   - **Atualizar uma rota existente**:
     - Endpoint: `PUT /api/v1/routes/{origin}/{destination}`
     - Corpo da requisição (JSON):
       ```json
       {
           "origin": "GRU",
           "destination": "JFK",
           "cost": 450
       }
       ```
     - Exemplo:
       ```
       PUT http://localhost:5000/api/v1/routes/GRU/JFK
       ```
   
   - **Deletar uma rota**:
     - Endpoint: `DELETE /api/v1/routes/{origin}/{destination}`
     - Exemplo:
       ```
       DELETE http://localhost:5000/api/v1/routes/GRU/JFK
       ```

### Testes Unitários

1. Para executar os testes, utilize o seguinte comando no diretório:
   `DesafioViagens\src\DesafioViagens`
   
   ```cmd
   dotnet test
   ```

2. Verifique a saída do console com os resultados dos testes.
