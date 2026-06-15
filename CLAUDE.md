# CLAUDE.md

Этот файл содержит инструкции для Claude Code (claude.ai/code) при работе с данным репозиторием.

## Команды

### Бэкенд (.NET 6)

```bash
dotnet build                                          # Сборка решения
dotnet test                                           # Запуск всех тестов
dotnet test tests/Hackathon.BL.Tests                  # Запуск одного тестового проекта
dotnet test --filter "FullyQualifiedName~MyTestName"  # Запуск конкретного теста
dotnet run --project src/Hackathon.API                # Запуск API
```

Интеграционные тесты требуют запущенной инфраструктуры. Перед запуском `Hackathon.Tests.Integration` установить `ASPNETCORE_ENVIRONMENT=Tests`.

### Миграции (EF Core)

Миграции применяются **автоматически при старте API** (`MigrationsTool.ApplyMigrations` в `src/Hackathon.API/Program.cs`). У каждого `DbContext` свои миграции в папке `Migrations/` соответствующего DAL-проекта (`Hackathon.DAL`, `Hackathon.Chats.DAL`, `Hackathon.Informing.DAL`, `Hackathon.Logbook.DAL`, `Hackathon.FileStorage.DAL`). При добавлении миграции обязательно указывать целевой контекст и проект:

```bash
dotnet ef migrations add MyMigration \
  --context ApplicationDbContext \
  --project src/Hackathon.DAL \
  --startup-project src/Hackathon.API
```

### Фронтенд (Angular 13)

```bash
cd src/Hackathon.UI
npm start          # Линтинг, установка зависимостей, запуск на http://localhost:4200
npm test           # Юнит-тесты через Karma/Jasmine
npm run lint       # ESLint с автоисправлением
npm run format     # Форматирование Prettier
```

### Инфраструктура (Docker)

```bash
docker-compose -f docker/docker-compose.yml up -d
```

| Сервис     | Порт  |
|------------|-------|
| PostgreSQL | 5433  |
| RabbitMQ   | 5676 / management 15676 |
| Redis      | 9379  |
| MinIO      | 9000 / консоль 9001 |
| Grafana    | 3000 (admin/admin123) |

### Конфигурация

Настройки читаются из `appsettings.{Environment}.json`. Строки подключения: `DefaultConnectionString` (PostgreSQL) и `MessageBroker` (RabbitMQ). Подробная инструкция по первому запуску — в [Wiki проекта](https://github.com/coder1coder/Hackathon/wiki/Первый-запуск).

## Стиль кода (нарушения — ошибки компиляции)

- Приватные readonly-поля обязательно с префиксом `_` (camelCase): `_myField`
- Все `if`/`else`/`for` и т.д. требуют фигурных скобок (`csharp_prefer_braces = true`)
- Primary constructors отключены — использовать явные конструкторы
- Нарушения именования (`IDE1006`) являются ошибками компиляции

## Архитектура

### Бэкенд

Бэкенд состоит из **ядра** и **подключаемых модулей**.

**Ядро** (`Hackathon.BL`, `Hackathon.DAL`, `Hackathon.Auth.*`, события/команды/пользователи/дружба) подключается напрямую в `Startup.cs` через методы-расширения `RegisterServices()`, `RegisterRepositories()`, `RegisterValidators()` и т.д. (`src/Hackathon.API/Startup.cs:87-94`); его SignalR-хабы маппятся прямо в `Startup.cs:217-218`.

**Подключаемые модули** — это самодостаточные фичи, реализующие `IApiModule` (`src/Hackathon.API.Module/IApiModule.cs`). Их ровно 4, и они регистрируются в `src/Hackathon.API/Program.cs`: `ChatsApiModule`, `InformingApiModule`, `FileStorageModule`, `LogbookApiModule`. Сам `Startup.cs` итерируется по списку модулей и вызывает их `ConfigureServices` / `ConfigureEndpoints`.

```csharp
public interface IApiModule {
    Assembly ConsumersAssembly { get; }          // MassTransit-консьюмеры
    IList<Func<IServiceProvider, DbContext>> RegisteredDbContextFactories { get; }
    void ConfigureServices(IServiceCollection, IConfiguration);
    void ConfigureEndpoints(IEndpointRouteBuilder, AppSettings);
}
```

**Стандартная структура модуля** (эталон — `Hackathon.Chats.*`):
```
Feature.Abstractions/    — интерфейсы и DTO (ITeamChatService, модели)
Feature.BL/              — реализации сервисов + FluentValidation-валидаторы
Feature.DAL/             — EF Core репозитории + DbContext + сущности
Feature.Infrastructure/  — MassTransit-консьюмеры, SignalR-хабы, провайдеры кэша
Feature.Module/          — контроллеры, точка входа ApiModule, Mapster-маппинги
```

**Контроллеры** наследуют `BaseController` (`src/Hackathon.API.Module/BaseController.cs`), который предоставляет `GetResult()` — оборачивает результат сервиса типа `Result<T>` в `IActionResult`, преобразуя `result.Errors.Type` (значение `HttpStatusCode`) в HTTP-статус ответа.

**Авторизация**: `BaseController` помечен `[Authorize]`, поэтому все эндпоинты по умолчанию требуют авторизации — открытые эндпоинты явно помечаются `[AllowAnonymous]` (см. `UserController`, `AuthController`). Для администраторских действий используется политика `[Authorize(Policy = nameof(UserRole.Administrator))]` (`Startup.cs:167-174`).

**Возвращаемый тип сервисов**: методы сервисов возвращают `Result<T>` или `Result` (из `BackendTools.Common.Models`) — для бизнес-ошибок уровня сервиса исключения не бросаются, возвращается неуспешный Result. Ошибки валидации (FluentValidation бросает `ValidationException` → 400) и непредвиденные исключения перехватываются глобальным обработчиком (`UseExceptionHandler` + `ExceptionActionFilter`, `Startup.cs:188,247-256`).

**База данных**: `ApplicationDbContext` (`src/Hackathon.DAL/`) содержит основные сущности (Users, Teams, Events, Projects). У каждого модуля свой DbContext (`ChatsDbContext`, `InformingDbContext`, `LogbookDbContext`, `FileStorageDbContext`). Используется EF Core + Npgsql (PostgreSQL). Маппинг объектов — Mapster, конфигурации в файлах `*.Mappings.cs`, сканируются автоматически.

**Общие интерфейсы сервисов** — `src/Hackathon.Common/Abstraction/`. Для межмодульных зависимостей использовать именно их, а не конкретные реализации.

**Асинхронный обмен сообщениями**: модули взаимодействуют через MassTransit + RabbitMQ. Контракты сообщений — records в `Hackathon.Common/Messages/` или в `Abstractions` каждого модуля. Консьюмеры реализуют `IConsumer<TMessage>` и обнаруживаются через `ConsumersAssembly`.

**Real-time (SignalR)**: хабы транслируют интеграционные события клиентам. Типичный поток: доменное событие → консьюмер MassTransit → вызов `IHubContext<THub>` → рассылка в именованную группу (например, `event-{id}-chat`). URL хабов настраиваются в `AppSettings.Hubs`. Отслеживание подключений к SignalR хранится в Redis через `IChatConnectionsProvider`.

### Фронтенд

Angular 13 + TypeScript. Все HTTP-клиенты наследуют `BaseApiClient` (`src/Hackathon.UI/src/app/clients/base.client.ts`), который подключает базовый URL и заголовки авторизации. HTTP-интерцептор (`common/interceptors/auth.interceptor.ts`) автоматически добавляет `Authorization: Bearer {token}` к каждому запросу и перенаправляет на страницу входа при 401.

Управление состоянием — **MobX** (декораторы `@observable`). Глобальные стор-объекты: `src/Hackathon.UI/src/app/shared/stores/` (`CurrentUserStore`, `ProfileUserStore`). Состояние загрузки на уровне компонентов — через `AppStateService`.

SignalR-подключения управляются централизованно через `signalr.service.ts` (`src/Hackathon.UI/src/app/services/`). Сервис аутентифицируется JWT-токеном и предоставляет коллбэки (например, `onEventChatNewMessage`), на которые подписываются компоненты.
