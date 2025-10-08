@echo off
echo Building Byway API for production...

REM Clean previous builds
if exist "bin" rmdir /s /q "bin"
if exist "obj" rmdir /s /q "obj"
if exist "publish" rmdir /s /q "publish"

REM Build the project
dotnet build --configuration Release

REM Publish for production
dotnet publish --configuration Release --output ./publish

echo Build completed successfully!
echo Published files are in the 'publish' folder.
echo Upload the contents of the 'publish' folder to SmarterASP.NET

pause
