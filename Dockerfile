# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY UserForm.API.sln .
COPY UserForm.API/UserForm.API.csproj UserForm.API/
COPY UserForm.BLL/UserForm.BLL.csproj UserForm.BLL/
COPY UserForm.DAL/UserForm.DAL.csproj UserForm.DAL/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR /src/UserForm.API
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserForm.API.dll"]
