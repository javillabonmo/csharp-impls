FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /app

# Copy csproj and restore distinct layers
COPY PersonalBlog.csproj ./
RUN dotnet restore

# Copy everything else and publish
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
WORKDIR /app

# Install ICU libraries for globalization support (needed for es-CO culture)
RUN apk add --no-cache icu-data-full icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "PersonalBlog.dll"]
