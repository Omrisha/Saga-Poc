#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SagaPoc.WebApplication/SagaPoc.WebApplication.csproj", "SagaPoc.WebApplication/"]
COPY ["SagaPoc.PersistanceModel/SagaPoc.PersistanceModel.csproj", "SagaPoc.PersistanceModel/"]
COPY ["SagaPoc.Sagas/SagaPoc.Sagas.csproj", "SagaPoc.Sagas/"]
COPY ["SagaPoc.Messages/SagaPoc.Messages.csproj", "SagaPoc.Messages/"]
RUN dotnet restore "SagaPoc.WebApplication/SagaPoc.WebApplication.csproj"
COPY . .
WORKDIR "/src/SagaPoc.WebApplication"
RUN dotnet build "SagaPoc.WebApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SagaPoc.WebApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SagaPoc.WebApplication.dll"]