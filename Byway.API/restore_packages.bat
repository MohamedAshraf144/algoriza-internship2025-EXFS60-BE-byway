@echo off
echo Restoring NuGet packages...
echo.

REM Restore packages
dotnet restore

echo.
echo Packages restored successfully!
echo.

REM Build project
echo Building project...
dotnet build

echo.
echo Build completed!
pause
