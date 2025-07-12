 
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

 
COPY *.sln ./
COPY ContactManagementProject/*.csproj ./ContactManagementProject/
RUN dotnet restore

 
COPY ContactManagementProject/. ./ContactManagementProject/
WORKDIR /app/ContactManagementProject
RUN dotnet publish -c Release -o /app/publish
 
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "ContactManagementProject.dll"]
