#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Discount.Gprc/Discount.Gprc.csproj", "Discount.Gprc/"]
COPY ["Discount.Infrastructure/Discount.Infrastructure.csproj", "Discount.Infrastructure/"]
COPY ["Discount.Domain/Discount.Domain.csproj", "Discount.Domain/"]
RUN dotnet restore "Discount.Gprc/Discount.Gprc.csproj"
COPY . .
WORKDIR "/src/Discount.Gprc"
RUN dotnet build "Discount.Gprc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Discount.Gprc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discount.Gprc.dll"]