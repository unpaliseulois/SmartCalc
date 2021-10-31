@echo off
setlocal EnableDelayedExpansion

if not "%1"=="" (
    set "solutionName=%~1"
	echo.
	@echo | set /p=Processing ...
	echo.
	echo.
	cd !solutionName!\Main && dotnet run Main.csproj && cd ..\..\    
    
) else (
	echo.
	echo Nothing to do. Missing parameter: 'dotnet solution name'.
	echo Please try again, but enter a valid solution name as parameter.
)
echo.
@echo | set /p=Press any key to continue ...
pause >nul
echo.	
exit /b





