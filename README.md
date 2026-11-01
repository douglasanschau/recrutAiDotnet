# recrutAiDotnet

**API REST para uma plataforma SaaS de recrutamento, desenvolvida com .NET 8, C#, MySQL e Stripe.**

O recrutAiDotnet é uma API desenvolvida como parte de uma plataforma SaaS voltada ao segmento de recrutamento e seleção, com funcionalidades para gerenciamento de empresas, autenticação de usuários e contratação de planos por assinatura.

O projeto tem como foco o desenvolvimento backend, aplicando conceitos de arquitetura em camadas, separação de responsabilidades, integração com banco de dados e serviços externos.

## 🚀 Funcionalidades

- **Cadastro de empresas:** Cadastro de empresas com validação de CNPJ e criação automática do primeiro usuário administrador.
- **Autenticação de usuários:** Login com validação de credenciais, armazenamento seguro de senhas com hash e autenticação via JWT.
- **Gerenciamento de perfis:** Suporte a diferentes perfis de acesso, como administrador e usuário comum.
- **Gerenciamento de empresas:** Controle de informações, status e planos contratados pelas empresas.
- **Gestão de assinaturas:** Integração com Stripe para criação de clientes, associação de métodos de pagamento e contratação de assinaturas recorrentes.
- **Planos de assinatura:** Suporte aos planos Gratuito, Standard e Premium.
- **Persistência de dados:** Integração com MySQL utilizando Dapper para execução de consultas SQL.
- **Validação de dados:** Validações utilizando Data Annotations e atributos personalizados.

## 🛠️ Tecnologias utilizadas

| Tecnologia | Aplicação |
|---|---|
| C# | Linguagem de programação |
| .NET 8 | Plataforma de desenvolvimento |
| ASP.NET Core Web API | Desenvolvimento da API REST |
| MySQL | Banco de dados relacional |
| Dapper | Mapeamento e acesso a dados |
| JWT Bearer | Autenticação baseada em tokens |
| Stripe.net | Integração de pagamentos e assinaturas |
| Swagger / OpenAPI | Documentação e exploração da API |
| Dependency Injection | Injeção de dependências |

## 🏗️ Arquitetura e organização do projeto

O projeto utiliza uma arquitetura em camadas, com separação das responsabilidades da aplicação, facilitando a manutenção, organização e evolução do código.

```text
recrutAiDotnet/
│
├── Configurations/
│   └── StripePlansConfiguration.cs
│
├── Controllers/
│   ├── UsuarioController.cs
│   ├── RecrutaiConfigController.cs
│   └── BaseController.cs
│
├── Data/
│   └── Database.cs
│
├── DependencyInjection/
│   └── ServiceCollectionExtensions.cs
│
├── Enums/
│
├── Helpers/
│
├── Models/
│
├── Repositories/
│   ├── CadastroRepository.cs
│   ├── EmpresaRepository.cs
│   └── UsuarioRepository.cs
│
├── Services/
│   ├── UsuarioService.cs
│   ├── JwtService.cs
│   └── StripeService.cs
│
└── Validators/
```

### Responsabilidades das camadas

- **Controllers:** Recebem as requisições HTTP, validam os dados de entrada e retornam as respostas da API.
- **Services:** Centralizam as regras de negócio e coordenam as operações da aplicação.
- **Repositories:** Responsáveis pelo acesso ao banco de dados, utilizando Dapper e consultas SQL.
- **Data:** Gerencia a criação das conexões com o banco de dados.
- **Models:** Representam as entidades e os dados utilizados pela aplicação.
- **Validators:** Implementam regras personalizadas de validação.
- **Configurations:** Centralizam configurações de serviços externos e planos de assinatura.
- **DependencyInjection:** Centraliza o registro e a configuração das dependências da aplicação.

## 💳 Integração com Stripe

A aplicação possui integração com o Stripe para viabilizar a contratação de planos por assinatura recorrente.

O fluxo de contratação contempla:

1. Recuperação do produto correspondente ao plano selecionado no Stripe.
2. Recuperação do preço recorrente ativo do produto.
3. Identificação do usuário autenticado e da empresa vinculada.
4. Criação de um cliente no Stripe.
5. Associação do método de pagamento ao cliente.
6. Criação da assinatura recorrente.
7. Atualização do plano contratado pela empresa quando a assinatura é identificada como ativa.

Os identificadores dos produtos Stripe são centralizados em uma configuração específica, facilitando o gerenciamento dos planos disponíveis.

## 🔌 Endpoints da API

| Método | Endpoint | Descrição |
|---|---|---|
| POST | `/api/cadastro` | Cadastro de empresa e usuário administrador |
| POST | `/api/login` | Autenticação de usuário e geração de JWT |
| POST | `/api/config/contratar_plano` | Contratação de plano via Stripe |
| POST | `/api/config/cadastrar_cartao_teste` | Criação de método de pagamento para testes no Stripe |

Os endpoints de configuração utilizam autenticação JWT.

## ⚙️ Como executar o projeto

### Pré-requisitos

Antes de executar a aplicação, é necessário possuir:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL](https://www.mysql.com/)
- [Conta no Stripe](https://stripe.com/) para utilização das funcionalidades de pagamento.
- Visual Studio, Visual Studio Code ou outra IDE compatível.

### 1. Clonar o repositório

```bash
git clone https://github.com/douglasanschau/recrutAiDotnet.git

cd recrutAiDotnet
```

### 2. Configurar as variáveis de ambiente

A aplicação necessita de configurações para conexão com o banco de dados, autenticação JWT e integração com Stripe.

Exemplo de configuração:

```bash
ConnectionStrings__DefaultConnection="Server=localhost;Database=recrutai;Uid=root;Pwd=sua_senha;"

Jwt__Key="sua_chave_secreta_jwt"

Stripe__SecretKey="sk_test_sua_chave_stripe"
```

Utilize suas próprias credenciais e nunca versione chaves ou informações sensíveis no repositório.

### 3. Configurar o banco de dados

Crie um banco de dados MySQL e configure a connection string correspondente.

A aplicação espera que as tabelas e os relacionamentos utilizados pelos repositórios estejam previamente criados.

### 4. Configurar os produtos no Stripe

Crie os produtos e respectivos preços recorrentes no painel do Stripe.

Atualize os identificadores dos produtos no arquivo:

```text
Configurations/StripePlansConfiguration.cs
```

Os produtos configurados devem corresponder aos planos utilizados pela aplicação.

### 5. Restaurar as dependências

```bash
dotnet restore
```

### 6. Executar a aplicação

```bash
dotnet run
```

Durante a execução no ambiente Development, a documentação Swagger fica disponível para exploração dos endpoints.

Endereço HTTP padrão:

```text
http://localhost:5010/swagger
```

## 🔐 Autenticação e segurança

A aplicação utiliza autenticação baseada em JWT (JSON Web Token).

Após a autenticação bem-sucedida, é gerado um token contendo informações de identificação do usuário.

Os tokens são assinados utilizando o algoritmo HMAC SHA-256 e possuem validade de oito horas.

O armazenamento e a verificação de senhas são realizados utilizando o `PasswordHasher` do ASP.NET Core.

## 📌 Conceitos técnicos aplicados

Durante o desenvolvimento do projeto, foram trabalhados conceitos importantes do desenvolvimento backend:

- Desenvolvimento de APIs REST com ASP.NET Core.
- Arquitetura em camadas e separação de responsabilidades.
- Injeção de dependências (Dependency Injection).
- Integração com banco de dados relacional utilizando Dapper.
- Escrita e execução de consultas SQL.
- Transações de banco de dados.
- Hash de senhas e autenticação JWT.
- Integração com serviços externos.
- Processamento de assinaturas e regras de negócio.
- Validação de dados com atributos personalizados.

## 👨‍💻 Autor

**Douglas Anschau Flores**

Desenvolvedor Full Stack | Backend Development

- [GitHub](https://github.com/douglasanschau)
- [Projeto recrutAiDotnet](https://github.com/douglasanschau/recrutAiDotnet)

---

*Projeto desenvolvido com foco em desenvolvimento backend, arquitetura de aplicações SaaS e integração com serviços externos.*
