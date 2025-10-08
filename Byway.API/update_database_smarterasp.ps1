# PowerShell script to update database on SmarterASP.NET
Write-Host "Updating database on SmarterASP.NET..." -ForegroundColor Green
Write-Host ""

# Set environment variable
$env:ASPNETCORE_ENVIRONMENT = "SmarterASP"

# Run Update-Database
dotnet ef database update --configuration SmarterASP

Write-Host ""
Write-Host "Database update completed!" -ForegroundColor Green
Read-Host "Press Enter to continue"
