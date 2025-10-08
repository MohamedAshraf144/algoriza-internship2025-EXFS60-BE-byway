@echo off
echo Updating database on SmarterASP.NET...
echo.

REM Set environment to use SmarterASP configuration
set ASPNETCORE_ENVIRONMENT=SmarterASP

REM Run Update-Database
dotnet ef database update --configuration SmarterASP

echo.
echo Database update completed!
pause
