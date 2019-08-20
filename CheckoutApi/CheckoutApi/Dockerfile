FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["CheckoutApi/CheckoutApi.csproj", "CheckoutApi/"]
RUN dotnet restore "CheckoutApi/CheckoutApi.csproj"
COPY . .
WORKDIR "/src/CheckoutApi"
RUN dotnet build "CheckoutApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CheckoutApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CheckoutApi.dll"]