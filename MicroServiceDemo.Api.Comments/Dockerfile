FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MicroServiceDemo.Api.Comments/MicroServiceDemo.Api.Comments.csproj", "MicroServiceDemo.Api.Comments/"]
COPY ["MicroServicesDemo.Core/MicroServicesDemo.Core.csproj", "MicroServicesDemo.Core/"]
RUN dotnet restore "MicroServiceDemo.Api.Comments/MicroServiceDemo.Api.Comments.csproj"
COPY . .
WORKDIR "/src/MicroServiceDemo.Api.Comments"
RUN dotnet build "MicroServiceDemo.Api.Comments.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MicroServiceDemo.Api.Comments.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MicroServiceDemo.Api.Comments.dll"]