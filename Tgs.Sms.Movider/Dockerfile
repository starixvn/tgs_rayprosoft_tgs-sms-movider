#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BuildConfiguration=Release
RUN --mount=type=secret,id=nuget,dst=/pat cp /pat ~/.nuget/NuGet/NuGet.Config
RUN dotnet nuget remove source 'Microsoft Visual Studio Offline Packages' || true
WORKDIR /src
COPY ["Tgs.Sms.Movider/Tgs.Sms.Movider.csproj", "Tgs.Sms.Movider/"]
COPY ["Tgs.Sms.Movider.Core/Tgs.Sms.Movider.Core.csproj", "Tgs.Sms.Movider.Core/"]
COPY ["Tgs.Sms.Movider.Repositories/Tgs.Sms.Movider.Repositories.csproj", "Tgs.Sms.Movider.Repositories/"]
RUN dotnet restore "Tgs.Sms.Movider/Tgs.Sms.Movider.csproj"
COPY . .
WORKDIR "/src/Tgs.Sms.Movider"
RUN dotnet build "Tgs.Sms.Movider.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.Sms.Movider.csproj" -c $BuildConfiguration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.Sms.Movider.dll"]