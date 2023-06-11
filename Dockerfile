FROM node:14 AS frontend

RUN npm -g i pnpm@6.32.15 husky

WORKDIR /app

COPY ./web/package.json ./web/package.json

RUN cd ./web && pnpm install

COPY . .

RUN cd ./web/ && npm run build

# =====================================================
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

RUN apt update && \
    apt install -y libgdiplus procps && \
    ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll && \
    apt clean

WORKDIR /docker

EXPOSE 5000

# =====================================================
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /build

COPY . .
COPY --from=frontend /app/web/dist /build/engine/src/Entrypoints/Seacraft/wwwroot

WORKDIR /build/engine

RUN dotnet restore ./src/Entrypoints/Seacraft/Seacraft.csproj --configfile ./.nuget/NuGet.Config

WORKDIR /build/engine/src/Entrypoints/Seacraft/
RUN dotnet build Seacraft.csproj -nowarn:cs1591 -c Release

# =====================================================
FROM build AS publish

RUN dotnet publish Seacraft.csproj -c Release -o /app

# =====================================================
FROM base AS final

WORKDIR /app

COPY --from=publish /app .
COPY --from=build /build/scripts/run.sh ./run.sh

ENV ASPNETCORE_URLS  http://*:5000
ENV TZ Asia/Shanghai

ENTRYPOINT ["bash", "run.sh"]
