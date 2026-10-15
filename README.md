# Тестовое задание Camera Monitoring

- [`Legacy`](Legacy/README.md) — code review и переработка загрузки событий из удалённого WCF-сервиса на .NET Framework 4.8;
- [`Modern`](Modern/README.md) — HTTP API на ASP.NET Core / .NET 10 для поиска ближайших активных камер в PostgreSQL

## Требования для запуска

- .NET 10 SDK;
- .NET Framework 4.8 для сборки Legacy-проекта;
- Docker для запуска PostgreSQL.

## Запуск Modern API

1. Запустить из корня приложения PostgreSQL и дождаться прохождения healthcheck:

```powershell
docker compose -f Modern/docker-compose.yml up -d --wait
```

PostgreSQL будет доступен на `localhost:15432`. Внутри контейнера используется стандартный порт `5432`; внешний порт изменён, чтобы не конфликтовать с локальной установкой PostgreSQL.

2. Запустить API:

```powershell
dotnet run --project Modern/CameraMonitoring.Api
```

При первом старте приложение создаст таблицу `Cameras` и добавит несколько демонстрационных записей. Swagger доступен по адресу:

```text
http://localhost:23638/swagger/index.html
```


## Основные решения

- Исходный Legacy-метод сохранён для сравнения, переработанный вариант находится рядом с ним
- Дубликаты событий удаляются через `HashSet` по ключу `(CameraId, Timestamp)`, после чего события сортируются по `Timestamp` и `CameraId`
- Modern API использует Controllers, EF Core и Npgsql без дополнительных CQRS/MediatR/AutoMapper-слоёв
- Код Modern API сгруппирован по фиче `Cameras`: controller и действие находятся рядом, а application-логика и PostgreSQL-инфраструктура разделены подпапками и интерфейсами
- Все асинхронные операции с базой получают `CancellationToken` запроса
- Запрос к базе строится через LINQ в EF Core
- Формула Haversine вынесена в `IDistanceCalculator` и покрыта тремя unit-тестами
- База создаётся и наполняется автоматически

## Что улучшить при наличии дополнительного времени

- заменить автоматический `EnsureCreatedAsync` на версионируемые EF Core migrations
- добавить фабрику WCF-клиентов и ограниченную retry-политику с пересозданием повреждённого канала
- добавить health checks, логирование и метрики
- для миллионов камер сначала ограничивать выборку в PostgreSQL по области координат и рассчитывать точное расстояние только для отобранных кандидатов
