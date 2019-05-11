FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["TelegramMid/TelegramMid.csproj", "TelegramMid/"]
RUN dotnet restore "TelegramMid/TelegramMid.csproj"
COPY . .
WORKDIR "/src/TelegramMid"
RUN dotnet build "TelegramMid.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TelegramMid.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TelegramMid.dll"]