@echo off
sc.exe delete "BCS.WebApi.Service"
set "CDir=%~dp0"
echo The exePath is: %~dp0
sc.exe create "BCS.WebApi.Service" start=auto binpath="%CDir%\BCS.WebApi.exe
sc.exe start "BCS.WebApi.Service"
