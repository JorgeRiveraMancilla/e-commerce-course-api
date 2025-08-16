FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["e-commerce-course-api.csproj", "./"]
RUN dotnet restore "e-commerce-course-api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "e-commerce-course-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "e-commerce-course-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "e-commerce-course-api.dll"]
