# План: завершение выделения модуля Auth

Цель — вынести логику авторизации и аутентификации из ядра (`Hackathon.BL/Auth`, контроллер в `Hackathon.API`) в самостоятельный подключаемый модуль `Auth`, реализующий `IApiModule`, по образцу `Logbook`/`Informing`.

Связанный анализ готовности — [modularization.md](modularization.md) (Auth — 🟡 средняя готовность).

> ## ✅ Статус: реализовано
>
> Модуль создан в `src/Hackathon.Auth/` (`Hackathon.Auth.Abstraction` / `Hackathon.Auth.BL` / `Hackathon.Auth.Module`), подключён в `Program.cs` (`new AuthApiModule()`), решение собирается, юнит-тесты `Hackathon.BL.Tests` проходят (30/30).
>
> **Обоснованные отклонения от плана:**
> - **Модели оставлены в `Hackathon.Common`** (`AuthTokenModel`, `SignInModel`, `SignInByGoogleModel`, `GenerateTokenPayload`, `AuthorizedUser`). Они потребляются вне Auth — `AuthTokenModel` использует `Hackathon.Client/IAuthApi`, `AuthorizedUser` — `BaseController` и контроллеры всех модулей. Перенос дал бы лишние ссылки и риск без архитектурной выгоды (см. оговорку в плане «при сомнении оставить в Common»). В модуль переехала только абстракция `IAuthService`.
> - **`SignInModelValidator` оставлен в `Hackathon.BL.Validation`** и регистрируется ядром. Он зависит от общего `PasswordValidator`; перенос затащил бы правила пароля в Auth. Модуль резолвит `IValidator<SignInModel>` из DI по интерфейсу.
> - **Зависимость от `Hackathon.BL.Users` устранена**: вместо `UserErrorMessages` в `Hackathon.Auth.BL` заведён `AuthErrorMessages` — модуль не ссылается на ядро BL.
> - **`services.Configure<AuthenticateSettings>(...)` оставлен в `Startup.cs`** (а не перенесён в модуль), так как настройки общие с конвейером проверки токена. `AddAuthentication`/`AddAuthorization` остаются в `Startup` (см. нюанс ниже).

## Текущее состояние

| Что | Где сейчас |
|-----|------------|
| `AuthService : IAuthService` | `src/Hackathon.BL/Auth/AuthService.cs` |
| `AuthTokenGenerator` (статический) | `src/Hackathon.BL/Auth/AuthTokenGenerator.cs` |
| `IAuthService` | `src/Hackathon.Common/Abstraction/Auth/IAuthService.cs` |
| Модели | `src/Hackathon.Common/Models/Auth/*` (`AuthorizedUser`, `GenerateTokenPayload`), `src/Hackathon.Common/Models/AuthTokenModel.cs` |
| Настройки | `src/Hackathon.Configuration/Auth/*` (`AuthenticateSettings`, `Internal/External/Google`) |
| Контроллер | `src/Hackathon.API/Controllers/AuthController.cs` (`SignIn`, `SignInByGoogle`) |
| Контракт запроса | `SignInRequest` в `Hackathon.API.Contracts.Users` + Mapster-маппинг |
| Конвейер аутентификации (JWT bearer) | `src/Hackathon.API/Extensions/AddAuthenticationExtension.cs` |
| Регистрация сервисов | `Hackathon.BL/ServiceCollectionExtensions.cs` (`IAuthService`, `IPasswordHashService`) |
| Регистрация настроек | `Startup.cs`: `services.Configure<AuthenticateSettings>(authSection)` + `AddAuthentication(authSettings)` |

### Зависимости `AuthService`

- `IPasswordHashService`, `IUserRepository`, `IUserService` — домен **Users** (всё через абстракции в `Hackathon.Common/Abstraction/User`);
- `IValidator<SignInModel>` (FluentValidation), `UserErrorMessages`, `CreateNewUserModel`;
- `Google.Apis.Auth` (валидация Google-токена), `AuthTokenGenerator`, `IOptions<AuthenticateSettings>`.

## Ключевой нюанс: выпуск токена ≠ конвейер проверки токена

`AuthService` **выпускает** JWT (sign-in). А `AddAuthenticationExtension` настраивает middleware, которое **проверяет** токен на каждом `[Authorize]`-эндпоинте всего приложения. Это сквозная инфраструктура.

> **Решение:** в модуль переносится только логика sign-in и генерация токена. Регистрация `AddAuthentication`/`AddAuthorization` остаётся централизованной в `Startup.cs` (либо модуль экспонирует extension-метод, но вызывается он по-прежнему из `Startup`, так как от него зависит авторизация всех остальных модулей).

## Целевая структура

Модулю **не нужен собственный `DbContext`** — он работает с данными пользователей через `IUserRepository`. Поэтому состав легче полноценного модуля:

```
src/Hackathon.Auth/
  Hackathon.Auth.Abstraction/   — IAuthService, модели (AuthTokenModel, GenerateTokenPayload, SignInModel/SignInByGoogleModel)
  Hackathon.Auth.BL/            — AuthService, AuthTokenGenerator, валидатор SignInModel
  Hackathon.Auth.Module/        — AuthController, AuthApiModule, Mapster-маппинг SignInRequest→SignInModel
```

`Hackathon.Auth.Module.csproj` ссылается на `Hackathon.API.Module`, `Hackathon.Auth.Abstraction`, `Hackathon.Auth.BL` и (для контрактов/настроек) `Hackathon.Common`, `Hackathon.Configuration`.

> Существующие пустые каталоги `Hackathon.Auth.*` (без `.csproj`, не входят в `.sln`) переиспользовать как место для проектов либо удалить и создать заново.

## Пошаговый план

### Этап 1. Создать проекты
1. Создать `Hackathon.Auth.Abstraction`, `Hackathon.Auth.BL`, `Hackathon.Auth.Module`, добавить в `.sln`.
2. Прописать `ProjectReference` по образцу `Hackathon.Logbook.Module.csproj`.

### Этап 2. Перенести абстракции и модели
3. Переместить `IAuthService` → `Hackathon.Auth.Abstraction`.
4. Переместить `AuthTokenModel`, `Models/Auth/*` (`GenerateTokenPayload`, `AuthorizedUser`*) и модели `SignInModel`/`SignInByGoogleModel` в `Hackathon.Auth.Abstraction`.
   - *`AuthorizedUser` может использоваться вне Auth (`BaseController`) — проверить зависимости перед переносом; при сомнении оставить в `Common`.*
5. Обновить `using` во всех потребителях (ядро по-прежнему может зависеть от `Hackathon.Auth.Abstraction`).

### Этап 3. Перенести бизнес-логику
6. Переместить `AuthService`, `AuthTokenGenerator` → `Hackathon.Auth.BL`.
7. Переместить валидатор `SignInModel` (найти в `Hackathon.BL.Validation`) → `Hackathon.Auth.BL`.
8. Зависимости на Users (`IUserRepository`, `IUserService`, `IPasswordHashService`) оставить через абстракции `Hackathon.Common/Abstraction/User` — менять не требуется.
9. `UserErrorMessages`/`CreateNewUserModel` — если они в `Hackathon.BL.Users`, вынести используемые сообщения в `Auth.BL` либо в `Common`, чтобы убрать ссылку на `Hackathon.BL`.

### Этап 4. Перенести контроллер и точку входа
10. Переместить `AuthController` → `Hackathon.Auth.Module`. Контроллер обнаружится автоматически как application part (сборка модуля ссылается на MVC через `Hackathon.API.Module`).
11. Создать `AuthApiModule : ApiModule`:
    ```csharp
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticateSettings>(configuration.GetSection("Auth"));
        services.AddScoped<IAuthService, AuthService>();
        // валидатор SignInModel
    }
    ```
12. `SignInRequest` (контракт) и его Mapster-маппинг перенести в `Hackathon.Auth.Module` (маппинги сканируются автоматически, т.к. сборки модулей попадают в `solutionAssemblies` в `Startup.cs:76`).

### Этап 5. Подключить модуль и почистить ядро
13. В `Program.cs` добавить `Modules.Add(new AuthApiModule());`.
14. Из `Hackathon.BL/ServiceCollectionExtensions.cs` убрать регистрацию `IAuthService` (строка `AddScoped<IAuthService, AuthService>()`).
15. В `Startup.cs` убрать `services.Configure<AuthenticateSettings>(...)`, если он переехал в модуль. **Оставить** `AddAuthentication(authSettings)`/`AddAuthorization(...)` — это сквозной конвейер (см. нюанс выше). Чтение секции `Auth` для middleware сохранить.
16. Удалить пустые файлы/каталоги из `Hackathon.BL/Auth`.

### Этап 6. Проверка
17. `dotnet build` (нарушения стиля = ошибки компиляции — следить за `_`-префиксами и фигурными скобками).
18. `dotnet test` — обновить namespace'ы в тестах Auth.
19. Ручная проверка: `POST /auth/signin`, `POST /auth/signinbygoogle`, доступ к `[Authorize]`-эндпоинту с полученным токеном.

## Риски и внимание

- **Сквозной конвейер аутентификации** — не переносить целиком в модуль (см. нюанс). Самая частая ошибка.
- **`AuthorizedUser`** может оказаться в ядре (`BaseController`) — переносить осторожно, иначе циклическая ссылка `API.Module → Auth.Abstraction`.
- **Зависимость от Users** остаётся жёсткой, но через абстракции — это допустимо до выделения модуля Users. После него контракты `IUserService`/`IUserRepository` должны остаться в общем слое абстракций.
- **DbContext не нужен** — миграций для Auth не добавляется.

## Оценка

Средняя сложность. Основной объём — механический перенос + правка namespace'ов; главный интеллектуальный момент — разделение «выпуск токена» (в модуль) и «проверка токена» (остаётся в `Startup`).
