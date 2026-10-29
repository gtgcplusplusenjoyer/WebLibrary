# Этап 1: Сборка приложения (используем SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем csproj файлы для восстановления зависимостей (кэширование)
COPY ["Library.API/Library.API.csproj", "Library.API/"]
COPY ["Library.Application/Library.Application.csproj", "Library.Application/"]
COPY ["Library.Domain/Library.Domain.csproj", "Library.Domain/"]
COPY ["Library.Infrastructure/Library.Infrastructure.csproj", "Library.Infrastructure/"]
RUN dotnet restore "Library.API/Library.API.csproj"

# Копируем весь остальной код и собираем
COPY . .
WORKDIR "/src/Library.API"
RUN dotnet publish "Library.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Этап 2: Запуск (используем только runtime, без SDK)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

# Копируем только скомпилированные файлы из этапа сборки
COPY --from=build /app/publish .

# Запускаем приложение
ENTRYPOINT ["dotnet", "Library.API.dll"]