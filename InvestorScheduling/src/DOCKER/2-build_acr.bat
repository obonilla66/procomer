@echo off

set ACR=acre01tsteaus

echo ==========================================
echo Publicando Agenda
echo ==========================================

az acr build ^
  --registry %ACR% ^
  --file docker/Dockerfile ^
  --target agenda ^
  --image agenda-api:1.0 ^
  .

echo ==========================================
echo Publicando PDF
echo ==========================================

az acr build ^
  --registry %ACR% ^
  --file docker/Dockerfile ^
  --target pdf ^
  --image pdf-api:1.0 ^
  .

echo ==========================================
echo Publicando Catalog
echo ==========================================

az acr build ^
  --registry %ACR% ^
  --file docker/Dockerfile ^
  --target catalog ^
  --image catalog-api:1.0 ^
  .

echo ==========================================
echo Publicando Web
echo ==========================================

az acr build ^
  --registry %ACR% ^
  --file docker/Dockerfile ^
  --target web ^
  --image web:1.0 ^
  .

echo.
echo Publicacion finalizada.
pause


