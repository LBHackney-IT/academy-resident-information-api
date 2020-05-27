FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./AcademyResidentInformationApi.sln ./
COPY ./AcademyResidentInformationApi/AcademyResidentInformationApi.csproj ./AcademyResidentInformationApi/
COPY ./AcademyResidentInformationApi.Tests/AcademyResidentInformationApi.Tests.csproj ./AcademyResidentInformationApi.Tests/

RUN dotnet restore ./AcademyResidentInformationApi/AcademyResidentInformationApi.csproj
RUN dotnet restore ./AcademyResidentInformationApi.Tests/AcademyResidentInformationApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out AcademyResidentInformationApi.Tests/AcademyResidentInformationApi.Tests.csproj

CMD dotnet test
