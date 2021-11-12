@echo off
cd SmartCalc/
dotnet build
cd Main
echo.
cls
dotnet run Main.csproj
cd ..
cd ..