# Кандидаты на выделение в модули

Документ фиксирует, какую функциональность ядра (`Hackathon.BL`, `Hackathon.DAL`, `Hackathon.Common`) потенциально можно вынести в самостоятельные подключаемые модули (`IApiModule`), и оценивает степень готовности каждого кандидата к переводу.

> Анализ основан на фактических зависимостях сервисов (конструкторы и `using`), а не только на структуре каталогов.

## Текущее состояние

Реальных модулей сейчас **четыре** (классы-наследники `ApiModule`):

| Модуль | Назначение |
|--------|------------|
| `ChatsApiModule` | Чаты событий и команд |
| `InformingApiModule` | Уведомления, e-mail, шаблоны |
| `FileStorageModule` | Хранение файлов и изображений |
| `LogbookApiModule` | Журнал действий |

Остальная доменная логика (события, команды, проекты, дружба, заявки, пользователи, авторизация) живёт в ядре и подключается напрямую в `Startup.cs`.

> **Замечание.** На диске присутствуют пустые каталоги `Hackathon.Auth.*` и `Hackathon.Users.*` (нет `.cs`, нет `.csproj`, отсутствуют в `.sln`) — следы незавершённого выделения Auth и Users в модули.

## Шкала готовности

| Уровень | Что означает |
|---------|--------------|
| 🟢 Высокая | Зависимости только через абстракции, минимум связей — отделяется почти «как есть» |
| 🟡 Средняя | Отделяется, но нужно развязать одну-две связи (общие константы, циклические зависимости) |
| 🔴 Низкая | Центральный хаб либо двусторонняя связанность — требуется существенный рефакторинг до переноса |
| ⚪ Не модуль | Логически принадлежит другому модулю или уже изолирован — отдельный модуль не нужен |

## Кандидаты

### 1. Projects (Проекты) — 🟢 Высокая

`ProjectService`, `ProjectRepository`, `ProjectEntity`.

Самый чистый кандидат. Сервис зависит только от `IProjectRepository`, валидаторов, `IGitHubIntegrationService` (уже отдельная сборка) и `IFileStorageService` (уже модуль). Связей с Teams/Events/Users на уровне сервиса нет. **Рекомендуется как первый для выделения.**

### 2. Friendship (Дружба / соц. граф) — 🟢 Высокая

`FriendshipService`, `FriendshipRepository`, плюс `UserProfileReaction*` (реакции на профиль).

Все зависимости через абстракции: `IFriendshipRepository`, `INotificationService` (Informing), `IUserService`, `IIntegrationEventsHub`. Самодостаточная социальная фича.

### 3. Teams (Команды) — 🟡 Средняя

`TeamService`, `PrivateTeamService`, `PublicTeamService`, `TeamRepository`, `TeamJoinRequestRepository`.

Зависимости через абстракции (`IEventRepository`, `IProjectRepository`, `IUserRepository`). Единственная жёсткая связь с Events — общая константа `EventsErrorMessages` (косметика, легко развязать).

### 4. Auth (Авторизация) — 🟡 Средняя

`Hackathon.BL/Auth` + соответствующие репозитории.

Логика в ядре, но абстракции частично подготовлены. Пустые каркасы `Hackathon.Auth.*` указывают на ранее начатое выделение. Требует аккуратной развязки с конвейером аутентификации в `Startup.cs`.

### 5. Users (Пользователи) — 🟡 Средняя

`UserService`, `PasswordHashService`, `UserProfileReactionService`, `UserRepository`.

Абстракции `IUserService` / `IUserRepository` уже вынесены в `Hackathon.Common/Abstraction/User` — это упрощает перенос. Сложность в том, что от пользователей зависит почти весь домен (Events, Teams, Friendship, ApprovalApplications), поэтому контракты придётся держать стабильными.

### 6. Events (Мероприятия) — 🔴 Низкая

`EventService`, `EventRepository`, `EventEntity`, `EventStageEntity`, `EventAgreementRepository` + фоновые задачи (`EventStartNotifierJob`, `PastEventStatusUpdateJob`, `StartedEventStatusUpdateJob`).

Центральный хаб домена: зависит от Team, User, ApprovalApplications, FileStorage, Informing. На `IEventRepository` завязаны Teams и ApprovalApplications. Высокая ценность, высокая трудоёмкость. Фоновые Job'ы выносить вместе с модулем.

### 7. ApprovalApplications (Заявки на согласование) — 🔴 Низкая

`ApprovalApplicationService`, `ApprovalApplicationRepository`.

Двусторонняя связь с Events: `EventService` зависит от `IApprovalApplicationService`, а сам сервис тянет `Common.Models.Event` / `IEventRepository`. Концептуально — универсальный модуль согласования, но **сначала нужно разорвать цикл с Event** (обобщить «объект заявки»).

## Пересмотренные пункты (не отдельные модули)

### Email / EmailConfirmation — ⚪ Влить в Informing

`EmailConfirmationService` опирается на `IEmailService` и `ITemplateService`, которые уже принадлежат модулю **Informing**. Логически это часть Informing, а не новый модуль.

### GitHub Integration — ⚪ Уже изолирован

`Hackathon.IntegrationServices.Github` — отдельная сборка, потребляется через `IGitHubIntegrationService`. Собственных контроллеров/endpoint'ов нет, оформлять как `IApiModule` нечего. Достаточно изолирован как сервисная библиотека.

## Сводка

| Кандидат | Готовность | Действие |
|----------|------------|----------|
| Projects | 🟢 Высокая | Выделять первым |
| Friendship | 🟢 Высокая | Выделять (включая реакции на профиль) |
| Teams | 🟡 Средняя | Развязать общую константу с Events |
| Auth | 🟡 Средняя | Завершить начатое выделение |
| Users | 🟡 Средняя | Удержать стабильные контракты `IUserService`/`IUserRepository` |
| Events | 🔴 Низкая | Дорого: расцепить зависимых, перенести Job'ы |
| ApprovalApplications | 🔴 Низкая | Сначала разорвать цикл с Event |
| Email/EmailConfirmation | ⚪ Не модуль | Влить в Informing |
| GitHub Integration | ⚪ Не модуль | Оставить как сервисную сборку |

## Рекомендуемый порядок

1. **Projects** и **Friendship** — низкий риск, чистые границы.
2. **Teams** — после устранения мелкой связи с Events.
3. **Users** и **Auth** — продолжить ранее начатое выделение, опираясь на готовые абстракции.
4. **Events** и **ApprovalApplications** — в последнюю очередь, после развязки взаимных зависимостей.

## Детальные планы

- [auth-module-extraction-plan.md](auth-module-extraction-plan.md) — завершение выделения модуля **Auth**.
- [email-confirmation-to-informing-plan.md](email-confirmation-to-informing-plan.md) — перенос подтверждения **Email** в модуль **Informing**.
