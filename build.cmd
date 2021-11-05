@echo off
cd SmartCalc/
dotnet build
cd SmartCalc.Tests
dotnet test SmartCalc.Tests.csproj
cd ..
cd ..