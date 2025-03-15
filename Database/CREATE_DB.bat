@echo off
setlocal

:: Set database connection details
set SERVER_NAME=localhost
set DATABASE_NAME=db
set USERNAME=sa
set PASSWORD=1

:: List all .sql files in the Table folder
::echo Listing all SQL files in Table folder:
::for %%f in (Table\*.sql) do (
::    echo Found: %%f
::)

echo Start
echo.

:: Loop through all .sql files in the Table folder and run them
for %%f in (Table\*.sql) do (
    echo Running %%f...
    sqlcmd -S %SERVER_NAME% -d %DATABASE_NAME% -U %USERNAME% -P %PASSWORD% -i "%%f"
    
    if %ERRORLEVEL% NEQ 0 (
        echo ERROR: Execution failed for %%f
        exit /b %ERRORLEVEL%
    )
)

echo.
echo All SQL scripts executed successfully!
echo.
echo End...
endlocal
pause
