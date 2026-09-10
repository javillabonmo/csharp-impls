FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /app

COPY PersonalBlog.csproj ./
#primero restauramos las dependencias para 
#que no se tenga que hacer cada vez que se haga un cambio en el código
RUN dotnet restore

COPY . ./
#luego si se toma todo el código y se compila
RUN dotnet publish -c Release -o out --no-restore

# ---------- Runtime(todo lo necesario para ejecutar la aplicación, NO para compilar) ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app

COPY --from=build /app/out .

# Usuario no root
RUN adduser -D appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "PersonalBlog.dll"]