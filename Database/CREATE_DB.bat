@echo off
setlocal

:: Set database connection details
set SERVER_NAME=localhost
set DATABASE_NAME=db
set USERNAME=sa
set PASSWORD=1
set SCRIPT_FILE=Tables\CARPARKS.Table.sql

:: Run the SQL script
echo Running %SCRIPT_FILE%...
sqlcmd -S %SERVER_NAME% -d %DATABASE_NAME% -U %USERNAME% -P %PASSWORD% -i "%CD%\%SCRIPT_FILE%"

if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Execution failed for %SCRIPT_FILE%
    exit /b %ERRORLEVEL%
)

echo SQL script executed successfully!
endlocal
pause
