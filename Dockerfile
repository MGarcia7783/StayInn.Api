# ===============================
# BUILD
# ===============================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["StayInn.slnx", "./"]
COPY ["StayInn.Api/StayInn.Api.csproj", "StayInn.Api/"]
COPY ["StayInn.Application/StayInn.Application.csproj", "StayInn.Application/"]
COPY ["StayInn.Domain/StayInn.Domain.csproj", "StayInn.Domain/"]
COPY ["StayInn.Infrastructure/StayInn.Infrastructure.csproj", "StayInn.Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "StayInn.slnx"

# Copiar todo el código y publicar
COPY . .

# Publicar la API
WORKDIR "/src/StayInn.Api"
RUN dotnet publish "StayInn.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===============================
# RUNTIME
# ===============================
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Librerías necesarias para la conexión a base de datos
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*

# Copiar archivos publicados
COPY --from=build /app/publish .

# Render usa variable PORT automáticamente
ENV ASPNETCORE_URLS=http://+:${PORT}
EXPOSE 8080

ENTRYPOINT ["dotnet", "StayInn.Api.dll"]