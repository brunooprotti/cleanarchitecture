FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["CleanArchitecture.Domain/CleanArchitecture.Domain.csproj", "CleanArchitecture.Domain/"]
COPY ["CleanArchitecture.Application/CleanArchitecture.Application.csproj", "CleanArchitecture.Application/"]
COPY ["CleanArchitecture.Infrastructure/CleanArchitecture.Infrastructure.csproj", "CleanArchitecture.Infrastructure/"]
COPY ["CleanArchitecture.Api/CleanArchitecture.Api.csproj", "CleanArchitecture.Api/"]
RUN dotnet restore "CleanArchitecture.Api/CleanArchitecture.Api.csproj"
COPY . .
WORKDIR "/src/CleanArchitecture.Api"
RUN dotnet build "CleanArchitecture.Api.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "CleanArchitecture.Api.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.Api.dll"]
