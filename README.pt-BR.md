# PestanaDevApi

🌐 [English](README.md) | **Português**

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12-239120?logo=csharp&logoColor=white)
![MariaDB](https://img.shields.io/badge/MariaDB-MySQL-003545?logo=mariadb&logoColor=white)
![SignalR](https://img.shields.io/badge/SignalR-tempo--real-0078D4)
![Testes](https://img.shields.io/badge/testes-xUnit-5E2750)

API REST por trás do meu site de portfólio e de outros projetos que desenvolvo. Ela cuida de autenticação (email/senha e Google, GitHub e LinkedIn), contas de usuário, confirmação de email, emails de contato, um **Scrum board** em tempo real e integrações com as APIs públicas da **NASA** e do **Metropolitan Museum of Art**.

---

## Sumário

- [Funcionalidades](#funcionalidades)
- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Como rodar](#como-rodar)
- [Configuração](#configuração)
- [Endpoints](#endpoints)
- [Board em tempo real (SignalR)](#board-em-tempo-real-signalr)
- [Traduções dos emails (i18n)](#traduções-dos-emails-i18n)
- [Jobs agendados](#jobs-agendados)
- [Tratamento de erros](#tratamento-de-erros)
- [Testes](#testes)
- [Autor](#autor)

---

## Funcionalidades

- **Autenticação**
  - Cadastro e login com email e senha (hash BCrypt)
  - Login / cadastro com **Google**, **GitHub** e **LinkedIn**
  - Tokens de acesso **JWT** + **refresh tokens** por dispositivo
  - **Google reCAPTCHA v3** nos formulários públicos
- **Confirmação de email**: códigos de 4 caracteres, salvos como hash HMAC-SHA256, com expiração e reenvio
- **Formulário de contato**: envia um email para o dono do site e um email de confirmação para o visitante, em **português ou inglês** (traduções em JSON, no estilo next-i18next)
- **Conta do usuário**: consultar, atualizar dados, trocar senha e excluir conta
- **Scrum board**
  - Boards com 5 colunas padrão, tarefas e participantes
  - Mover tarefas entre colunas e reordenar colunas
  - Permissões de dono/participante
  - **Atualizações em tempo real** para todos os participantes via **SignalR**
  - Controle de concorrência (semaphore) e cache em memória
- **NASA**: Foto Astronômica do Dia (um dia ou um período)
- **Metropolitan Museum of Art**: busca de obras de arte
- **Infraestrutura**: cache em memória, rate limiting (500 requisições / 30 min por IP), jobs agendados (Quartz) e tratamento global de exceções

## Tecnologias

| Área | Tecnologia |
|---|---|
| Runtime / framework | .NET 8, ASP.NET Core Web API |
| Banco de dados | MariaDB |
| Acesso a dados | Dapper + MySqlConnector (SQL puro, sem ORM) |
| Tempo real | ASP.NET Core SignalR |
| Autenticação | JWT Bearer, Google.Apis.Auth, GitHub OAuth, LinkedIn OpenID Connect |
| Segurança | BCrypt.Net, HMAC-SHA256, Google reCAPTCHA v3, rate limiting |
| Jobs agendados | Quartz.NET (persistido no MariaDB) |
| Emails | System.Net.Mail (SMTP) + templates HTML + traduções em JSON |
| Documentação | Swagger (Swashbuckle) |
| Testes | xUnit + Moq |

## Arquitetura

O código é dividido em camadas. Cada camada só conversa com a de baixo, por meio de interfaces registradas no container de injeção de dependência.

```
Controllers  →  Services  →  Repositories  →  MariaDB
   (HTTP)       (regras de     (Dapper +
                 negócio)       SQL puro)
```

```
PestanaDevApi/
├── AppConfig/          # Configuração de banco, Quartz e segredos
├── Constants/          # Mensagens, queries SQL (classes parciais), chaves de cache etc.
├── Controllers/        # Endpoints HTTP
├── Dtos/               # DTOs de requisição, resposta e notificação
├── EmailTemplates/     # Templates HTML dos emails
├── Exceptions/         # ApiException
├── Extensions/         # Extension methods (DTO → parâmetros SQL, validações)
├── Hubs/               # Hubs do SignalR (BoardHub)
├── Interfaces/         # Contratos de services, repositórios e hubs
├── Locales/            # Traduções (en/email.json, pt/email.json)
├── Models/             # Modelos de domínio e enums
├── QuartzJobs/         # Jobs agendados
├── Repositories/       # Acesso a dados (Dapper)
├── Services/           # Regras de negócio, cache, notificações, integrações
└── Utils/              # Helpers (hash, datas, cliente HTTP etc.)

PestanaDevApi.Tests/    # Testes unitários (xUnit + Moq)
```

Convenções:

- **Respostas**: todo service retorna um DTO que herda de `DefaultResponseDto` (`IsSuccess`, `StatusCode`, `Message`); os controllers transformam falhas em respostas HTTP com `HandleFailure()`.
- **SQL**: as queries ficam em `Constants/Queries` como constantes. `BoardQueries` é uma classe parcial dividida em `Selects`, `Inserts`, `Updates` e `Deletes`.
- **Mensagens de erro**: cada uma tem um código único (`"6A3C : Board was not found!"`), o que facilita rastrear a origem.

## Como rodar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- MariaDB (ou MySQL) com dois bancos: um da aplicação e um do Quartz
- Credenciais dos serviços externos que você for usar (SMTP, Google, GitHub, LinkedIn, reCAPTCHA, NASA)

### Banco de dados

Principais tabelas da aplicação:

| Tabela | Uso |
|---|---|
| `USERS_PROFILE_DATA` | Usuários |
| `USERS_PROFILE_PLATFORM_DATA` | Vínculo do usuário com Google/GitHub/LinkedIn |
| `CONFIRMATION_CODE` | Códigos de confirmação de email (hash) |
| `CONFIRMED_EMAILS` | Emails confirmados |
| `REFRESH_TOKEN` | Refresh tokens por dispositivo |
| `BOARD`, `BOARD_USERS`, `BOARD_COLUMNS`, `BOARD_TASKS` | Scrum board |

> 🔒 **O script de criação do banco é privado** e não está neste repositório. Para obtê-lo, solicite ao dono do projeto ([Vitor Pestana](https://github.com/vitorpestana94)).

As tabelas do Quartz são criadas com o [script oficial do Quartz.NET para MySQL](https://github.com/quartznet/quartznet/blob/main/database/tables/tables_mysql_innodb.sql).

### Executando

```bash
git clone https://github.com/vitorpestana94/PestanaDevApi.git
cd PestanaDevApi

# crie os arquivos de configuração (veja "Configuração")

dotnet run --project PestanaDevApi --launch-profile http
```

A API sobe em `http://localhost:5024`. No ambiente `Development`, o Swagger fica em `http://localhost:5024/swagger`.

## Configuração

As configurações são lidas de arquivos `.properties` (`chave=valor`), que **não são versionados** (estão no `.gitignore`):

```
PestanaDevApi/config/
├── app/
│   ├── config.properties                # produção
│   └── config.Development.properties    # desenvolvimento
└── app-secret/
    ├── config.properties
    └── config.Development.properties
```

O arquivo é escolhido pelo ambiente: `config.{Ambiente}.properties` para `Development` e `Local`, e `config.properties` nos demais.

**`config/app`**

```properties
projectname=PestanaDevApi
api.baseUrl=https://url-da-sua-api
database.url=localhost
database.username=seu-usuario
database.databasename=seu-banco
database.quartz.databasename=seu-banco-quartz
google.clientid.web=seu-google-client-id
github.appname=nome-do-seu-app-github
github.endpoint=https://api.github.com
linkedin.publickeysurl=https://www.linkedin.com/oauth/openid/jwks
linkedin.validissuser=https://www.linkedin.com/oauth
linkedin.clientid=seu-linkedin-client-id
email.address=seu-email@dominio.com
email.apppassword=sua-senha-de-app-smtp
email.smtp=smtp.seu-provedor.com
nasa.endpoint=https://seu-endpoint-apod-da-nasa
metropolitanMuseum.endpoint=https://collectionapi.metmuseum.org/public/collection
```

**`config/app-secret`**

```properties
database.password=senha-do-banco
jwt.key=chave-base64-de-no-minimo-32-bytes
jwt.issuer=seu-issuer
sha.secret=segredo-do-hmac-dos-codigos
captchaSecret=seu-segredo-do-recaptcha-v3
```

> ⚠️ Nunca faça commit desses arquivos. O `jwt.key` precisa estar em Base64.

## Endpoints

🔒 = exige o header `Authorization: Bearer <jwt>`.

### Autenticação — `/auth`

| Método | Rota | Descrição | |
|---|---|---|---|
| POST | `/auth` | Login com email e senha (reCAPTCHA) | |
| POST | `/auth/oauth` | Login / cadastro com Google, GitHub ou LinkedIn | |
| POST | `/auth/signup` | Cadastro (exige email confirmado) | |
| GET | `/auth/isEmailRegistered/{email}` | Verifica se o email já está cadastrado | |
| POST | `/auth/forgot-password` | Redefine a senha (exige email confirmado) | |
| POST | `/auth/refresh-token` | Gera um novo token a partir do refresh token | |
| DELETE | `/auth` | Logout (apaga o refresh token do dispositivo) | 🔒 |

### Email e confirmação — `/email`, `/confirmation`

| Método | Rota | Descrição |
|---|---|---|
| POST | `/email/contact` | Envia o email de contato (dono + visitante) |
| POST | `/email/confirmation` | Envia um código de confirmação |
| POST | `/email/confirmation/resend` | Reenvia o código de confirmação |
| POST | `/confirmation` | Valida o código recebido |
| GET | `/confirmation/sent/{email}` | Verifica se já foi enviado um código |

### Usuário — `/user` 🔒

| Método | Rota | Descrição |
|---|---|---|
| GET | `/user` | Dados do usuário logado |
| PATCH | `/user` | Atualiza nome e/ou email |
| PATCH | `/user/password` | Troca a senha |
| DELETE | `/user` | Exclui a conta |

### Scrum board — `/board` 🔒

| Método | Rota | Descrição | Quem pode |
|---|---|---|---|
| POST | `/board` | Cria um board (5 colunas + primeira tarefa) | Qualquer usuário |
| GET | `/board` | Lista os boards do usuário | Dono |
| GET | `/board/{id}` | Dados do board (colunas, tarefas, participantes) | — |
| PATCH | `/board/{boardId}` | Atualiza nome/descrição | Dono |
| DELETE | `/board/{boardId}` | Exclui o board | Dono |
| PATCH | `/board/{boardId}/tasks/{taskId}` | Atualiza ou move uma tarefa | Participante |
| DELETE | `/board/{boardId}/tasks/{taskId}` | Exclui uma tarefa | Participante |
| PATCH | `/board/{boardId}/columns/{columnId}` | Renomeia ou reordena uma coluna | Participante |
| GET | `/board/{boardId}/members` | Lista os participantes | Participante |
| POST | `/board/{boardId}/members` | Adiciona um participante pelo email | Dono |
| DELETE | `/board/{boardId}/members/{userId}` | Remove um participante (ou sai do board) | Dono / o próprio participante |

### Integrações

| Método | Rota | Descrição | |
|---|---|---|---|
| GET | `/nasa?date=yyyy-MM-dd` | Foto Astronômica do Dia (hoje, se não houver data) | 🔒 |
| GET | `/nasa/period?startDate=...&endDate=...` | Fotos astronômicas de um período | 🔒 |
| GET | `/metropolitan-museum?search=...` | Busca de obras de arte | |

## Board em tempo real (SignalR)

As alterações no board continuam sendo feitas pelos endpoints REST. O SignalR só **envia** essas alterações para os outros participantes.

- **Hub:** `/hubs/board` (exige o JWT, enviado como `access_token` na query string)
- **Métodos que o cliente chama:** `JoinBoard(boardId)` (só participantes) e `LeaveBoard(boardId)`
- **Payload de todo evento:** `{ boardId, changedBy, data }`

| Evento | `data` | Enviado após |
|---|---|---|
| `BoardUpdated` | board | `PATCH /board/{id}` |
| `BoardDeleted` | `boardId` | `DELETE /board/{id}` |
| `TaskUpdated` | tarefa atualizada | `PATCH .../tasks/{taskId}` |
| `TaskDeleted` | `taskId` | `DELETE .../tasks/{taskId}` |
| `ColumnsUpdated` | todas as colunas | `PATCH .../columns/{columnId}` |
| `MemberAdded` | `{ id, name, email, isOwner }` | `POST .../members` |
| `MemberRemoved` | `memberId` | `DELETE .../members/{userId}` |

Exemplo de cliente em TypeScript:

```ts
import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl(`${API_URL}/hubs/board`, { accessTokenFactory: () => getAccessToken() })
  .withAutomaticReconnect()
  .build();

connection.on("TaskUpdated", (n) => {
  if (n.changedBy !== myUserId) upsertTask(n.data); // ignora as próprias alterações
});

await connection.start();
await connection.invoke("JoinBoard", boardId);
connection.onreconnected(() => connection.invoke("JoinBoard", boardId));
```

> Conexões, grupos, semaphore e cache ficam em memória, então a configuração atual suporta **uma única instância da API**. Para rodar mais instâncias seria preciso um backplane (ex.: Redis).

## Traduções dos emails (i18n)

Os textos dos emails ficam em arquivos JSON, seguindo as convenções do **next-i18next**:

```
Locales/
├── en/email.json
└── pt/email.json
```

```csharp
_translationService.Translate("pt", "email:contact.client.title", new() { ["name"] = "Ana" }); // "Olá, Ana!"
```

- Chaves no formato `namespace:caminho.da.chave`
- Interpolação com `{{variavel}}`
- Usa o inglês quando o idioma ou a chave não existem

Para adicionar um idioma, crie `Locales/{idioma}/email.json` com as mesmas chaves.

## Jobs agendados

| Job | Horário (UTC) | Função |
|---|---|---|
| `DeleteUnfreshConfirmationCodesJob` | A cada 15 minutos | Apaga códigos de confirmação expirados |
| `DeleteExpiredRefreshTokensJob` | Todo dia às 3:00 | Apaga refresh tokens expirados |
| `NasaCacheRefreshJob` | Todo dia às 4:00 | Atualiza o cache da NASA |
| `MetropolitanMuseumCacheRefreshJob` | Todo dia às 4:30 | Atualiza o cache do Metropolitan Museum |

## Tratamento de erros

Os erros voltam no seguinte formato:

```json
{ "message": "6A3C : Board was not found!" }
```

| Situação | Status |
|---|---|
| Requisição inválida / regra de negócio | 400 |
| Não autenticado / credenciais inválidas | 401 |
| Sem permissão / reCAPTCHA reprovado | 403 |
| Recurso não encontrado | 404 |
| Erro inesperado | 500 |

Erros de validação do modelo retornam `{ "status": 400, "message": "Validation error" }`.

## Testes

O projeto `PestanaDevApi.Tests` tem testes unitários (xUnit + Moq) dos services, extensions e utils. Eles não precisam de banco de dados nem de rede.

```bash
dotnet test
```

## Autor

**Vitor Pestana**, [GitHub](https://github.com/vitorpestana94)
