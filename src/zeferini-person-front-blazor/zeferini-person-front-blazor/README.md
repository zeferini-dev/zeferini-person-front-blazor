# TesteFull - Blazor Server

Aplicação front-end desenvolvida em **Blazor Server** com **.NET 10** e **MudBlazor** como biblioteca de componentes UI.

## 🚀 Tecnologias

- **.NET 10** - Framework de desenvolvimento
- **Blazor Server** - Framework de UI com interatividade do lado do servidor
- **MudBlazor** - Biblioteca de componentes Material Design para Blazor
- **Docker** - Containerização

## 📁 Estrutura do Projeto

```
first/
├── Components/
│   ├── Layout/
│   │   └── MainLayout.razor      # Layout principal com navbar e drawer
│   ├── Pages/
│   │   ├── Home.razor            # Página inicial
│   │   ├── PersonList.razor      # Lista de pessoas
│   │   └── PersonForm.razor      # Formulário de cadastro/edição
│   ├── Shared/
│   │   └── ConfirmDialog.razor   # Diálogo de confirmação reutilizável
│   ├── App.razor                 # Componente raiz
│   ├── Routes.razor              # Configuração de rotas
│   └── _Imports.razor            # Imports globais
├── Models/
│   └── Person.cs                 # Modelos de dados
├── Services/
│   ├── ApiConfiguration.cs       # Configuração das URLs das APIs
│   └── PersonService.cs          # Serviço de comunicação com APIs
├── wwwroot/                      # Arquivos estáticos
├── appsettings.json              # Configurações da aplicação
├── Dockerfile                    # Configuração para Docker
├── Program.cs                    # Ponto de entrada da aplicação
└── first.csproj                  # Arquivo de projeto
```

## 🏗️ Arquitetura CQRS

A aplicação implementa o padrão **CQRS** (Command Query Responsibility Segregation):

- **Commands (escrita)** → API MySQL na porta 3000
- **Queries (leitura)** → API MongoDB na porta 3001

## 🔧 Configuração

### appsettings.json

```json
{
  "Api": {
    "CommandUrl": "http://localhost:3000",
    "QueryUrl": "http://localhost:3001"
  }
}
```

## 🛠️ Como Executar

### Desenvolvimento Local

```bash
# Restaurar dependências
dotnet restore

# Executar em modo desenvolvimento
dotnet run

# Ou com hot reload
dotnet watch run
```

A aplicação estará disponível em: `https://localhost:5001` ou `http://localhost:5000`

### Com Docker

```bash
# Build da imagem
docker build -t testefull-blazor .

# Executar container
docker run -p 8080:80 testefull-blazor
```

## 📋 Funcionalidades

### Gerenciamento de Pessoas (CRUD)

- ✅ Listagem de pessoas em tabela
- ✅ Criação de nova pessoa
- ✅ Edição de pessoa existente
- ✅ Exclusão com confirmação via diálogo
- ✅ Feedback via Snackbar
- ✅ Loading states
- ✅ Validação de formulários

### Interface

- ✅ Layout responsivo com MudBlazor
- ✅ Navbar com menu hamburguer
- ✅ Drawer lateral com navegação
- ✅ Tema Material Design
- ✅ Ícones Material Icons
- ✅ Links para APIs externas

## 🔄 Comparação Angular ↔ Blazor

| Angular | Blazor |
|---------|--------|
| Components | Razor Components |
| Services | Services (DI) |
| Angular Material | MudBlazor |
| RxJS Observables | Events / async-await |
| HttpClient | HttpClient |
| Reactive Forms | EditForm |
| Router | Blazor Router |

## 📦 Dependências

- `MudBlazor` - Componentes UI Material Design

## 🐳 Docker Compose

Para adicionar ao docker-compose.yml principal:

```yaml
blazor-first:
  build:
    context: ./front/blazor/first
    dockerfile: Dockerfile
  ports:
    - "4202:80"
  environment:
    - ASPNETCORE_ENVIRONMENT=Production
    - Api__CommandUrl=http://nest-first:3000
    - Api__QueryUrl=http://mongo-sync:3001
  depends_on:
    - nest-first
    - mongo-sync
```

## 📝 Licença

MIT
