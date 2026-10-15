# Modern API

ASP.NET Core API на .NET 10 возвращает ближайшие активные камеры из PostgreSQL

## Запуск

1. Из корня репозитория сначала необходимо запустить PostgreSQL:

```powershell
docker compose -f Modern/docker-compose.yml up -d --wait
```

PostgreSQL публикуется на `localhost:15432`

2. Затем запустить API:

```powershell
dotnet run --project Modern/CameraMonitoring.Api
```

При первом запуске API создаёт таблицу и несколько демонстрационных камер. Swagger доступен по адресу `http://localhost:23638/swagger/index.html`

Пример запроса:

```http
POST http://localhost:23638/api/cameras/nearest
Content-Type: application/json

{
  "latitude": 55.7558,
  "longitude": 37.6173,
  "limit": 3
}
```


## Структура

Код организован по функциональности `Cameras`, а не разнесён по глобальным техническим папкам:

```text
Cameras/
├── CamerasController.cs
├── CameraServiceCollectionExtensions.cs
├── Camera.cs
├── Actions/
│   └── GetNearest/
│       ├── GetNearestCamerasHandler.cs
│       └── Models/
├── Services/
│   ├── ICameraRepository.cs
│   ├── IDistanceCalculator.cs
│   └── HaversineDistanceCalculator.cs
└── Infrastructure/
    ├── CameraDbContext.cs
    ├── PostgresCameraRepository.cs
    └── DatabaseInitializer.cs
```

`Actions` содержит application-сценарий, `Services` — его абстракции и вычислительную логику, а `Infrastructure` — детали EF Core и PostgreSQL. `GetNearestCamerasHandler` вызывается контроллером напрямую, поэтому для одного сценария не добавлялись MediatR, CQRS и AutoMapper

## Принятые решения

- Входные значения проверяются средствами ASP.NET Core и DataAnnotations. При непрохождении валидации результат будет `400 Bad Request`
- Endpoint передаёт работу в handler и не содержит формулу расчёта расстояния или доступ к базе данных
- `IDistanceCalculator` зарегистрирован как singleton, потому что реализация не хранит изменяемого состояния
- `CameraDbContext`, repository и handler зарегистрированы как scoped
- EF Core-запрос использует `AsNoTracking` - т.к. данные с базы тянем только для чтения, фильтрует неактивные камеры в PostgreSQL и принимает `CancellationToken`. EF Core формирует параметризованный SQL
- `EnsureCreatedAsync` и runtime seed используются только для простого запуска тестового проекта. В реальном проекте схема должна изменяться миграциями

## Если камер 5 миллионов

Загружать все пять миллионов камер в приложение нельзя. Сначала я бы выбирал в PostgreSQL только активные камеры из небольшого диапазона широты и долготы вокруг заданной точки. Если найденных камер меньше переданного `limit`, то диапазон поиска постепенно бы увеличивался. Далее расстояние рассчитывалось бы только для найденных кандидатов, после чего они сортировались бы и ограничивались по `limit`.

## Что улучшить при дополнительном времени

- заменить `EnsureCreatedAsync` на миграции
- добавить юнит тесты на хендлер, контроллер, репозиторий 
- добавить обработку временной недоступности PostgreSQL и health checks
- добавить логирование и метрики времени выполнения запроса

