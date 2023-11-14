# Hackathon
[![Tests](https://github.com/coder1coder/Hackathon/actions/workflows/tests.yml/badge.svg?branch=develop)](https://github.com/coder1coder/Hackathon/actions/workflows/tests.yml)

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
cd .\Hackathon.API\
dotnet ef database update
cd ../ (вернуться в директорию Hackathon.sln)
dotnet build
dotnet run --no-build
```

Запустить UI клиентское приложение
```
Скачиваем и устанавливаем node.js не ниже v16
Стабильная версия для проекта v16.15.1
cd .\Hackathon.UI\
npm install -g @angular/cli (установка Angular модуля)
npm install (установка зависимостей)
ng serve (запуск dev-сервера, перейдите по адресу http://localhost:4200/)
```
[Скачать Node.js](https://nodejs.org/en/download)<br>
[Скачать Node.js v16.15.1](https://nodejs.org/dist/v16.15.1/)
