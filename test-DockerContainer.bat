@echo off
echo Checking if all required containers are running...

:: Check MongoDB
docker ps | findstr /I "mongo"
IF %ERRORLEVEL% NEQ 0 (
    echo MongoDB container is NOT running!
) ELSE (
    echo MongoDB is running.
)

:: Check Redis
docker ps | findstr /I "redis_server"
IF %ERRORLEVEL% NEQ 0 (
    echo Redis container is NOT running!
) ELSE (
    echo Redis is running.
)

:: Check RabbitMQ
docker ps | findstr /I "rabbitmq"
IF %ERRORLEVEL% NEQ 0 (
    echo RabbitMQ container is NOT running!
) ELSE (
    echo RabbitMQ is running.
)

echo.
echo Done!
pause
