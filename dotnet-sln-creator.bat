@echo off
setlocal EnableDelayedExpansion

if not "%1"=="" (
    set "slnName=%~1"    
    mkdir !slnName!
    cd !slnName!
    echo.
    dotnet new sln -n !slnName!
    dotnet new console -n "Main"
    :: Add a created project to the main sln file 
    dotnet sln !slnName!.sln add ./Main/Main.csproj  
    :: Add a library to the project
    dotnet new classlib -n "Global" 
    :: Add a created library to the main sln file
    dotnet sln !slnName!.sln add ./Global/Global.csproj
    :: Add reference to the created libaray 
    dotnet add Main/Main.csproj reference Global/Global.csproj
    :: clean creted library
    del Global\Class1.cs
    echo Global library cleaned.
    echo Open '!slnName!' solution directory in vscode. 
    cd Main
    Code .
) else (
	echo.
	echo Nothing to do. Missing parameter: solution name.
	echo Please try again, but enter a valid solution name as parameter.
)	
exit /b


