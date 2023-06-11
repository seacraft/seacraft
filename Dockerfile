FROM dotnet/core/aspnet:3.1 AS base

RUN sed -i -e "s@http://[^/]* @http://mirror.sy/debian-security @" -e "s@http://[^/]*/@http://mirror.sy/@" /etc/apt/sources.list && \
    apt update && apt install -y procps && apt clean

RUN apt update && apt-get install libgdiplus -y && ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll

WORKDIR /docker

EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /build
COPY . .
RUN dotnet restore ./server/src/Entrypoints/Seacraft.Web/Seacraft.Web.csproj --configfile ./.nuget/NuGet.config
WORKDIR ./server/src/Entrypoints/Seacraft.Web/
RUN dotnet build Seacraft.Web.csproj.csproj  -nowarn:cs1591 -c Release

FROM build AS publish
RUN dotnet publish Seacraft.Web.csproj.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=build /build/utility/run.sh ./run.sh

ENV ASPNETCORE_URLS  http://*:5000
ENV TZ Asia/Shanghai

ENTRYPOINT ["bash", "run.sh"]
