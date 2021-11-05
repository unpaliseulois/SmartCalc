@echo off

setlocal
set yello=[33m
set strongYellow=[93m
set strongCyan=[96m

cmd /c start /MAX /i cmd /c "build-sln.cmd && echo %strongCyan% && set /p choice = "Press any key to exit..." && pause"

endlocal