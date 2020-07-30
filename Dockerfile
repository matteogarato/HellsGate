#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8082
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["HellsGate.MVC/HellsGate.MVC.csproj", "HellsGate.MVC/"]
COPY ["HellsGate.Api/HellsGate.Api.csproj", "HellsGate.Api/"]
RUN dotnet restore "HellsGate.MVC/HellsGate.MVC.csproj"
COPY . .
WORKDIR "/src/HellsGate.MVC"
RUN dotnet build "HellsGate.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HellsGate.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HellsGate.MVC.dll"]

RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh