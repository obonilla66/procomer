1-build_local.bat:
@echo off

echo ==========================================
echo Construyendo Agenda
echo ==========================================

docker build ^
  -f Dockerfile ^
  --target agenda ^
  -t agenda-api:1.0 ^
  ..

echo ==========================================
echo Construyendo PDF
echo ==========================================

docker build ^
  -f Dockerfile ^
  --target pdf ^
  -t pdf-api:1.0 ^
  ..

echo ==========================================
echo Construyendo Catalog
echo ==========================================

docker build ^
  -f Dockerfile ^
  --target catalog ^
  -t catalog-api:1.0 ^
  ..

echo ==========================================
echo Construyendo Web
echo ==========================================

docker build ^
  -f Dockerfile ^
  --target web ^
  -t web:1.0 ^
  ..

echo.
echo Build finalizado.
pause
---

