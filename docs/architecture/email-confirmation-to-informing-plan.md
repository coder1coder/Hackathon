# План: перенос подтверждения Email в модуль Informing

Цель — переместить логику подтверждения e-mail из ядра в существующий модуль **Informing**, которому она логически принадлежит (он уже владеет отправкой писем и шаблонами). Это не создание нового модуля, а **вливание** в `InformingApiModule`.

Связанный анализ готовности — [modularization.md](modularization.md) (Email/EmailConfirmation — ⚪ «не отдельный модуль, влить в Informing»).

## Почему именно Informing

`EmailConfirmationService` уже зависит от сервисов, которые принадлежат Informing:

- `IEmailService` → `Hackathon.Informing.BL/Services/EmailService.cs`
- `ITemplateService` → `Hackathon.Informing.BL/Services/TemplateService.cs`
- шаблон `Templates.EmailConfirmationRequest` → `Hackathon.Informing.Abstractions/Constants/Templates.cs`
- содержимое шаблона уже наполняется миграцией Informing: `20240206064845_Informing_Update_EmailConfirmationRequestTemplate`

То есть половина зависимостей уже на стороне Informing — перенос убирает обратную связь «ядро → Informing».

## Текущее состояние

| Что | Где сейчас |
|-----|------------|
| `EmailConfirmationService : IEmailConfirmationService` | `src/Hackathon.BL/Email/EmailConfirmationService.cs` |
| `EmailConfirmationErrorMessages` | `src/Hackathon.BL/Email/EmailConfirmationErrorMessages.cs` |
| `IEmailConfirmationService`, `IEmailConfirmationRepository` | `src/Hackathon.Common/Abstraction/User/*` |
| Модели | `src/Hackathon.Common/Models/Users/EmailConfirmationRequestModel.cs`, `EmailConfirmationRequestParameters.cs`, `UserEmailModel.cs` |
| **Сущность** | `src/Hackathon.DAL/Entities/EmailConfirmationRequestEntity.cs` |
| **Конфигурация EF** | `src/Hackathon.DAL/Configurations/EmailConfirmationRequestEntityConfiguration.cs` |
| **DbSet** | `EmailConfirmations` в `ApplicationDbContext` (ядро) |
| Репозиторий | `src/Hackathon.DAL/Repositories/EmailConfirmationRepository.cs` (использует `ApplicationDbContext`) |
| Миграция создания таблицы | `src/Hackathon.DAL/Migrations/20221224164028_EmailConfirmation` |
| Эндпоинты | `UserController`: `POST profile/email/confirm`, `POST profile/email/confirm/request` |
| Регистрация | `Hackathon.BL/ServiceCollectionExtensions.cs` (сервис), `Hackathon.DAL/ServiceCollectionExtensions.cs` (репозиторий) |
| Настройки | `EmailSettings.EmailConfirmationRequestLifetime` (`Hackathon.Configuration`) — Informing уже читает `EmailSettings` |

## Главная сложность: смена владельца таблицы между DbContext'ами

Таблица подтверждений сейчас принадлежит `ApplicationDbContext`, а должна перейти под `InformingDbContext`. При этом **оба контекста используют одну и ту же физическую БД** (`InformingApiModule` регистрирует `InformingDbContext` на `DefaultConnectionString`, см. `InformingApiModule.cs:44`). Миграции применяются автоматически на старте (`MigrationsTool`).

Опасность: наивный перенос приведёт к тому, что новая миграция Informing попытается **создать** уже существующую таблицу, а миграция ядра — **удалить** её.

> **Стратегия (без потери данных):**
> 1. В `ApplicationDbContext` добавить миграцию, которая «забывает» таблицу на уровне модели, **но не делает `DropTable`** (вручную вычистить `Up()` от удаления данных — оставить только обновление снапшота модели).
> 2. В `InformingDbContext` добавить миграцию, которая в `Up()` **не создаёт** таблицу заново (тело no-op / только синхронизация снапшота), поскольку таблица физически уже есть.
> 3. Каждый шаг — отдельным коммитом, с ручной проверкой сгенерированного `Up()/Down()` перед применением.
>
> Альтернатива (чистая, но дороже): сознательная миграция данных — создать таблицу под Informing, перелить строки, удалить старую. Оправдана, только если допустим даунтайм/потеря активных кодов подтверждения (они короткоживущие, время жизни — `EmailConfirmationRequestLifetime`).

## Целевое размещение

```
Hackathon.Informing.Abstractions/
  Services/IEmailConfirmationService.cs
  Repositories/IEmailConfirmationRepository.cs
  Models/  EmailConfirmationRequestModel, EmailConfirmationRequestParameters
Hackathon.Informing.BL/
  Services/EmailConfirmationService.cs
  EmailConfirmationErrorMessages.cs
Hackathon.Informing.DAL/
  Entities/EmailConfirmationRequestEntity.cs
  Configurations/EmailConfirmationRequestEntityConfiguration.cs
  Repositories/EmailConfirmationRepository.cs  (на InformingDbContext)
  + DbSet EmailConfirmations в InformingDbContext
Hackathon.Informing.Module/
  Controllers/EmailConfirmationController.cs
```

## Пошаговый план

### Этап 1. Абстракции и модели
1. Переместить `IEmailConfirmationService`, `IEmailConfirmationRepository` из `Hackathon.Common/Abstraction/User` → `Hackathon.Informing.Abstractions`.
2. Переместить модели `EmailConfirmationRequestModel`, `EmailConfirmationRequestParameters` → `Hackathon.Informing.Abstractions/Models`.
   - `UserEmailModel` проверить на использование вне подтверждения; если используется в Users — оставить в `Common`.
3. Обновить `using` у потребителей.

### Этап 2. Бизнес-логика
4. Переместить `EmailConfirmationService`, `EmailConfirmationErrorMessages` → `Hackathon.Informing.BL`.
5. Зависимости `IEmailService`/`ITemplateService` станут **внутримодульными** (упрощение).
6. Решить зависимость на `IUserRepository` (сервис читает пользователя по id ради email/имени):
   - **Вариант A (быстрый):** `Hackathon.Informing.BL` ссылается на `Hackathon.Common/Abstraction/User` (`IUserRepository`) — допустимо, абстракция общая.
   - **Вариант B (чистый):** убрать зависимость, передавая email/имя пользователя в параметрах вызова (вызывающий код уже в контексте пользователя). Предпочтительно для развязки.

### Этап 3. Данные (DAL)
7. Переместить `EmailConfirmationRequestEntity` и `EmailConfirmationRequestEntityConfiguration` → `Hackathon.Informing.DAL`.
8. Добавить `DbSet<EmailConfirmationRequestEntity> EmailConfirmations` в `InformingDbContext`; убрать из `ApplicationDbContext`.
9. Переместить `EmailConfirmationRepository` → `Hackathon.Informing.DAL/Repositories`, переключить на `InformingDbContext`.
10. Сгенерировать миграции по стратегии выше:
    ```bash
    dotnet ef migrations add EmailConfirmation_RemoveFromCore \
      --context ApplicationDbContext --project src/Hackathon.DAL --startup-project src/Hackathon.API
    dotnet ef migrations add EmailConfirmation_AdoptInInforming \
      --context InformingDbContext \
      --project src/Hackathon.Informing/Hackathon.Informing.DAL --startup-project src/Hackathon.API
    ```
    **Вручную отредактировать `Up()/Down()`** обеих миграций, чтобы не дропать/не пересоздавать существующую таблицу (см. стратегию).

### Этап 4. Контроллер
11. Создать `EmailConfirmationController` в `Hackathon.Informing.Module`, перенеся туда два действия из `UserController`.
    - **Сохранить маршруты** `profile/email/confirm` и `profile/email/confirm/request` для обратной совместимости API/UI; `AuthorizedUserId` доступен через `BaseController`.
12. Убрать `IEmailConfirmationService` и соответствующие действия из `UserController` (`src/Hackathon.API/Controllers/UserController.cs:99-108`).

### Этап 5. Регистрация и чистка
13. В `InformingApiModule.ConfigureServices` добавить:
    ```csharp
    serviceCollection.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
    serviceCollection.AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>();
    ```
14. Убрать регистрацию сервиса из `Hackathon.BL/ServiceCollectionExtensions.cs` и репозитория из `Hackathon.DAL/ServiceCollectionExtensions.cs`.
15. Удалить каталог `Hackathon.BL/Email` и осиротевшие файлы из ядра.
16. `EmailSettings` Informing уже читает — убедиться, что `EmailConfirmationRequestLifetime` доступен модулю (секция та же).

### Этап 6. Проверка
17. `dotnet build` (стиль = ошибки компиляции).
18. `dotnet test`, особое внимание — `Hackathon.DAL.Tests` и тесты подтверждения e-mail (обновить namespace'ы).
19. **Проверка миграций на копии БД с данными**: убедиться, что таблица и строки сохранились, дублирующего создания/дропа нет.
20. Ручная проверка: `POST profile/email/confirm/request` → письмо отправлено, запись создана; `POST profile/email/confirm` с кодом → статус подтверждён.

## Риски и внимание

- **Перенос таблицы между DbContext'ами одной БД** — главный риск. Обязательно ручная правка миграций и проверка на копии с данными.
- **Совместимость API** — сохранить существующие маршруты, иначе сломается UI.
- **Зависимость Informing → Users** — предпочесть Вариант B (передача данных), чтобы не плодить обратные связи.
- **Шаблон письма** уже в Informing — дублировать не нужно.

## Оценка

Средняя сложность; технически безопасный перенос кода, но требует аккуратной работы с миграциями EF Core из-за смены владельца таблицы в общей базе.
