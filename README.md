# Subscription Control Backend

Fertiges ASP.NET Core Web API Backend für Subscription-Control mit Entity Framework Core und SQL Server.

## Enthalten

- EF-Core `AppDbContext` mit Tabellen für `Users`, `NotificationSettings`, `Categories`, `BillingCycles`, `Subscriptions`, `Notifications`
- Beziehungen gemäß Diagramm: User 1:n Subscription, User 1:1 NotificationSettings, Subscription n:1 Category, Subscription n:1 BillingCycle, Subscription 1:n Notification
- Seed-Daten über `HasData` im `AppDbContext`
- Zusätzliches SQL-Insert-Skript unter `Scripts/seed.sql`
- CRUD-Endpunkte für Users, Categories, BillingCycles, Subscriptions, Notifications
- Endpunkte für NotificationSettings pro User
- Request-Beispiele in `Subscription_Control_Backend.http`
- Docker Compose mit SQL Server und Backend

## Start lokal mit Docker

```bash
docker compose up --build
```

API: `http://localhost:8080`

## Start lokal ohne Docker

SQL Server muss laufen. Connection String steht in `appsettings.json`.

```bash
dotnet restore
dotnet run
```

## Wichtige Demo-IDs

- Demo User: `11111111-1111-1111-1111-111111111111`
- Streaming Category: `22222222-2222-2222-2222-222222222221`
- Monthly Billing Cycle: `33333333-3333-3333-3333-333333333331`
- Netflix Subscription: `44444444-4444-4444-4444-444444444441`

## API-Beispiele

Alle Beispiele liegen in `Subscription_Control_Backend.http` und können direkt in Rider/Visual Studio/VS Code REST Client ausgeführt werden.

## Mapper-Struktur

Die zentrale `EntityMapper`-Klasse wurde entfernt. Stattdessen gibt es pro Aggregat/Entity einen eigenen Mapper:

- `UserMapper`
- `SubscriptionMapper`
- `CategoryMapper`
- `BillingCycleMapper`
- `NotificationMapper`
- `NotificationSettingsMapper`

Die Update-Methoden heißen jetzt bewusst `UpdateEntity(...)` statt `Apply(...)`, damit klar ist, dass ein bestehendes EF-Core-Tracking-Objekt aktualisiert wird.
