@echo off
echo Fixing Admin Issues...
echo.

echo 1. Restoring NuGet packages...
dotnet restore

echo.
echo 2. Building project...
dotnet build

echo.
echo 3. Running database update...
dotnet ef database update

echo.
echo 4. Starting application...
echo You can now test the AdminSetup endpoints in Swagger
echo.

pause
