#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SagaPoc.StateMachineWebApp/SagaPoc.StateMachineWebApp.csproj", "SagaPoc.StateMachineWebApp/"]
COPY ["SagaPoc.Sagas/SagaPoc.Sagas.csproj", "SagaPoc.Sagas/"]
COPY ["SagaPoc.Messages/SagaPoc.Messages.csproj", "SagaPoc.Messages/"]
RUN dotnet restore "SagaPoc.StateMachineWebApp/SagaPoc.StateMachineWebApp.csproj"
COPY . .
WORKDIR "/src/SagaPoc.StateMachineWebApp"
RUN dotnet build "SagaPoc.StateMachineWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SagaPoc.StateMachineWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SagaPoc.StateMachineWebApp.dll"]