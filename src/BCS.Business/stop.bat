@echo off
set serviceName=BCS.WebApi.Service

sc query %serviceName% | find "RUNNING"
if %errorlevel% equ 0 (
    echo Stopping %serviceName%...
    net stop %serviceName%
) else (
    echo %serviceName% is not running or doesn't exist.
)
