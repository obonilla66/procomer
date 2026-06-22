# Comandos útiles

## Restaurar y compilar

```powershell
dotnet restore
dotnet build
```

## Ejecutar API

```powershell
dotnet run --project Agenda.Api
```

## Crear migración

```powershell
dotnet ef migrations add InitialCreate `
  --project Agenda.Infrastructure `
  --startup-project Agenda.Api `
  --context AgendaDbContext
```

## Aplicar migración

```powershell
dotnet ef database update `
  --project Agenda.Infrastructure `
  --startup-project Agenda.Api `
  --context AgendaDbContext
```
