#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Trainer.csproj", "."]
COPY ["../Trainer.DAL/Trainer.DAL.csproj", "../Trainer.DAL/"]
COPY ["../Trainer.BLL/Trainer.BLL.csproj", "../Trainer.BLL/"]
RUN dotnet restore "./Trainer.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Trainer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Trainer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Trainer.dll"]