#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

ARG OPENEHR_DB
ARG OPENEHR_USER
ARG OPENEHR_PASSWD
ENV OPENEHR_DB $OPENEHR_DB
ENV OPENEHR_USER $OPENEHR_USER
ENV OPENEHR_PASSWD $OPENEHR_PASSWD

WORKDIR /app
EXPOSE 9787
WORKDIR /src
COPY ["SmICSWebApp/SmICSWebApp.csproj", "SmICSWebApp/"]
COPY ["SmICSCoreLib/SmICSCoreLib.csproj", "SmICSCoreLib/"]
COPY ["SmICSConnection.Test/SmICSConnection.Test.csproj", "SmICSConnection.Test/"]
COPY ["SmICSDataGenerator.Test/SmICSDataGenerator.Test.csproj", "SmICSDataGenerator.Test/"]
COPY ["SmICS.Tests/SmICSFactory.Tests.csproj", "SmICS.Tests/"]
COPY ["TestData/", "TestData/"]
RUN dotnet restore "SmICSWebApp/SmICSWebApp.csproj"

WORKDIR /src/.
COPY . .
RUN dotnet build "SmICSWebApp/SmICSWebApp.csproj" -c Release -o /app/build

FROM build AS publish
COPY . ./
RUN dotnet publish "SmICSWebApp/SmICSWebApp.csproj" -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
COPY --from=publish /app/out .
ENTRYPOINT ["dotnet", "SmICSWebApp.dll"]
