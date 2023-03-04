# Hackathon
[![.NET](https://github.com/coder1coder/Hackathon/actions/workflows/main.yml/badge.svg)](https://github.com/coder1coder/Hackathon/actions/workflows/main.yml)

Сервис позволяет организовать хакатон, все самые полезные функции в одном инструменте.

[Описание общих терминов](https://github.com/coder1coder/Hackathon/wiki)

[Фичи в планах](https://github.com/coder1coder/Hackathon/discussions/categories/ideas)

[Статус текущей разработки](https://github.com/coder1coder/Hackathon/projects/1)

Запустить сервисы
```
docker-compose up -d
```

Настроить секреты
```
"EmailSettings": {
    "EmailSender": {
        "Server": "smtp.host.ru",
        "Port": 587,
        "Username": "username@host.ru",
        "Password": "password",
        "EnableSsl": true,
        "Sender": "username@host.ru"
    }
}
```
[Как настроить секреты?](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)

Запустить веб-апи
```
dotnet build
dotnet run --no-build
```

Запустить клиентское приложение
```
cd .\Hackathon.UI\src\
ng serve
```
