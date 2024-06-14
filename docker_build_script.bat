
REM Stop containers
for /f "tokens=*" %%i in ('docker ps -q --filter "name=fetchcityworkerservice"') do docker stop %%i

REM Remove containers
for /f "tokens=*" %%i in ('docker ps -aq --filter "name=fetchcityworkerservice"') do docker rm %%i

docker build -f FetchCityWorkerService/Dockerfile -t fetchcityworkerservice .



REM Stop containers
for /f "tokens=*" %%i in ('docker ps -q --filter "name=treatcityworkerservice"') do docker stop %%i

REM Remove containers
for /f "tokens=*" %%i in ('docker ps -aq --filter "name=treatcityworkerservice"') do docker rm %%i

docker build -f TreatCityWorkerService/Dockerfile -t treatcityworkerservice .


REM Stop containers
for /f "tokens=*" %%i in ('docker ps -q --filter "name=bankworkerservice"') do docker stop %%i

REM Remove containers
for /f "tokens=*" %%i in ('docker ps -aq --filter "name=bankworkerservice"') do docker rm %%i

docker build -f BankWorkerService/Dockerfile -t bankworkerservice .

REM Stop containers
for /f "tokens=*" %%i in ('docker ps -q --filter "name=treatannouncementworkerservice"') do docker stop %%i

REM Remove containers
for /f "tokens=*" %%i in ('docker ps -aq --filter "name=treatannouncementworkerservice"') do docker rm %%i

docker build -f TreatAnnouncementWorkerService/Dockerfile -t treatannouncementworkerservice .



REM Stop containers
for /f "tokens=*" %%i in ('docker ps -q --filter "name=fetchannoucementworkerservice"') do docker stop %%i

REM Remove containers
for /f "tokens=*" %%i in ('docker ps -aq --filter "name=fetchannoucementworkerservice"') do docker rm %%i

docker build -f FetchAnnoucementWorkerService/Dockerfile -t fetchannoucementworkerservice .


REM Stop containers
for /f "tokens=*" %%i in ('docker ps -q --filter "name=rentworkerservice"') do docker stop %%i

REM Remove containers
for /f "tokens=*" %%i in ('docker ps -aq --filter "name=rentworkerservice"') do docker rm %%i

docker build -f RentWorkerService/Dockerfile -t rentworkerservice .

for /f "tokens=*" %%i in ('docker ps -q --filter "name=rentalinvestmentaidweb"') do docker stop %%i

REM Remove containers
for /f "tokens=*" %%i in ('docker ps -aq --filter "name=rentalinvestmentaidweb"') do docker rm %%i

docker build -f RentalInvestmentAid.Web/Dockerfile -t rentalinvestmentaidweb .

