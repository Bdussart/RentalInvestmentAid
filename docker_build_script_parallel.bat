@echo off
setlocal enabledelayedexpansion

REM Stop and remove containers in series
call :StopAndRemove fetchcityworkerservice
call :StopAndRemove treatcityworkerservice
call :StopAndRemove bankworkerservice
call :StopAndRemove treatannouncementworkerservice
call :StopAndRemove fetchannoucementworkerservice
call :StopAndRemove rentworkerservice
call :StopAndRemove rentalinvestmentaidweb

REM Build images 
call :BuildImage FetchCityWorkerService/Dockerfile fetchcityworkerservice 
call :BuildImage TreatCityWorkerService/Dockerfile treatcityworkerservice
call :BuildImage BankWorkerService/Dockerfile bankworkerservice
call :BuildImage TreatAnnouncementWorkerService/Dockerfile treatannouncementworkerservice
call :BuildImage FetchAnnoucementWorkerService/Dockerfile fetchannoucementworkerservice
call :BuildImage RentWorkerService/Dockerfile rentworkerservice
call :BuildImage RentalInvestmentAid.Web/Dockerfile rentalinvestmentaidweb


:StopAndRemove
set serviceName=%1
echo Stopping and removing containers for %serviceName% and delete image
for /f "tokens=*" %%i in ('docker ps -q --filter "name=%serviceName%"') do docker stop %%i
for /f "tokens=*" %%i in ('docker ps -aq --filter "name=%serviceName%"') do docker rm %%i
for /f "tokens=*" %%i in ('docker images -q %serviceName%') do docker rmi %%i
goto :eof

:BuildImage
set dockerfile=%1
set imageName=%2
echo Building image %imageName% from %dockerfile%
docker build -f %dockerfile% -t %imageName% .
goto :eof
