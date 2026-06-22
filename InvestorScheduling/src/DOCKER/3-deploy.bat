@echo off

REM :::::::::::::::::::::::::::::
REM Despliegue Microservicios
REM :::::::::::::::::::::::::::::

set RG=rsgr-E01-TST-EaUS
set LOCATION=eastus

set SQLHOST=rs-dbs-pte01-tst-4e4d-eaus-1.database.windows.net
set SQLDB=procomer
set SQLUSER=usr_e01_tst
set SQLPASSWORD=Bx92Kp2025!

set ACR=acre01tsteaus
set ENV=cae-e01-tst-eaus

set CONNECTIONSTRING=Server=tcp:%SQLHOST%,1433;Initial Catalog=%SQLDB%;Persist Security Info=False;User ID=%SQLUSER%;Password=%SQLPASSWORD%;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;

echo ==========================================
echo Creando ACR
echo ==========================================

az acr create ^
  --resource-group %RG% ^
  --name %ACR% ^
  --sku Basic ^
  --admin-enabled true

echo ==========================================
echo Creando Container Apps Environment
echo ==========================================

az containerapp env create ^
  --name %ENV% ^
  --resource-group %RG% ^
  --location %LOCATION%

echo ==========================================
echo Obteniendo Login Server
echo ==========================================

FOR /F "delims=" %%i IN ('az acr show --name %ACR% --query loginServer --output tsv') DO SET ACRLOGIN=%%i

echo ACR=%ACRLOGIN%

echo ==========================================
echo Creando Catalog API
echo ==========================================

az containerapp create ^
  --name catalog-api ^
  --resource-group %RG% ^
  --environment %ENV% ^
  --image %ACRLOGIN%/catalog-api:1.0 ^
  --registry-server %ACRLOGIN% ^
  --target-port 8080 ^
  --ingress external ^
  --min-replicas 1 ^
  --max-replicas 3 ^
  --env-vars ConnectionStrings__AgendaDb="%CONNECTIONSTRING%" ASPNETCORE_URLS=http://+:8080

echo ==========================================
echo Creando Agenda API
echo ==========================================

az containerapp create ^
  --name agenda-api ^
  --resource-group %RG% ^
  --environment %ENV% ^
  --image %ACRLOGIN%/agenda-api:1.0 ^
  --registry-server %ACRLOGIN% ^
  --target-port 8080 ^
  --ingress external ^
  --min-replicas 1 ^
  --max-replicas 3 ^
  --env-vars ConnectionStrings__AgendaDb="%CONNECTIONSTRING%" ASPNETCORE_URLS=http://+:8080

echo ==========================================
echo Creando PDF API
echo ==========================================

az containerapp create ^
  --name pdf-api ^
  --resource-group %RG% ^
  --environment %ENV% ^
  --image %ACRLOGIN%/pdf-api:1.0 ^
  --registry-server %ACRLOGIN% ^
  --target-port 8080 ^
  --ingress external ^
  --min-replicas 1 ^
  --max-replicas 3 ^
  --env-vars ConnectionStrings__AgendaDb="%CONNECTIONSTRING%" ASPNETCORE_URLS=http://+:8080

echo ==========================================
echo Obteniendo URLs
echo ==========================================

FOR /F "delims=" %%i IN ('az containerapp show --name catalog-api --resource-group %RG% --query properties.configuration.ingress.fqdn --output tsv') DO SET CATALOGURL=%%i

FOR /F "delims=" %%i IN ('az containerapp show --name agenda-api --resource-group %RG% --query properties.configuration.ingress.fqdn --output tsv') DO SET AGENDAURL=%%i

FOR /F "delims=" %%i IN ('az containerapp show --name pdf-api --resource-group %RG% --query properties.configuration.ingress.fqdn --output tsv') DO SET PDFURL=%%i

echo Catalog=%CATALOGURL%
echo Agenda=%AGENDAURL%
echo PDF=%PDFURL%

echo ==========================================
echo Creando Frontend
echo ==========================================

az containerapp create ^
  --name web ^
  --resource-group %RG% ^
  --environment %ENV% ^
  --image %ACRLOGIN%/web:1.0 ^
  --registry-server %ACRLOGIN% ^
  --target-port 8080 ^
  --ingress external ^
  --min-replicas 1 ^
  --max-replicas 3 ^
  --env-vars ^
  CATALOG_API_URL=https://%CATALOGURL% ^
  AGENDA_API_URL=https://%AGENDAURL% ^
  PDF_API_URL=https://%PDFURL% ^
  ASPNETCORE_URLS=http://+:8080

echo.
echo ==========================================
echo DESPLIEGUE FINALIZADO
echo ==========================================
pause