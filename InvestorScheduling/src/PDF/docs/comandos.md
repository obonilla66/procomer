# Comandos

## Compilar

```powershell
dotnet restore
dotnet build
```

## Ejecutar

```powershell
dotnet run --project .\PDF.Api\PDF.Api.csproj
```

## Probar

```powershell
Invoke-WebRequest `
  -Uri "https://localhost:7001/api/pdf/agendas/1" `
  -OutFile "agenda-1.pdf" `
  -SkipCertificateCheck
```

Cambie el puerto según el que muestre la consola.
