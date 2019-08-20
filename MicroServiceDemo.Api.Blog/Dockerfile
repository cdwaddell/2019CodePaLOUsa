FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MicroServiceDemo.Api.Blog/MicroServiceDemo.Api.Blog.csproj", "MicroServiceDemo.Api.Blog/"]
COPY ["MicroServicesDemo.Core/MicroServicesDemo.Core.csproj", "MicroServicesDemo.Core/"]
RUN dotnet restore "MicroServiceDemo.Api.Blog/MicroServiceDemo.Api.Blog.csproj"
COPY . .
WORKDIR "/src/MicroServiceDemo.Api.Blog"
RUN dotnet build "MicroServiceDemo.Api.Blog.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MicroServiceDemo.Api.Blog.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MicroServiceDemo.Api.Blog.dll"]