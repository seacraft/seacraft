FROM node:14 AS frontend

RUN npm -g i pnpm@6.32.15 husky

WORKDIR /app

COPY ./web/package.json ./web/package.json

RUN cd ./web && pnpm install

COPY . .

RUN cd ./web/ && npm run build

# =====================================================
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

RUN apt update && apt install -y \
    libgdiplus procps && \
    ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll \
    apt clean

WORKDIR /docker

EXPOSE 5000

# =====================================================
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /build

COPY . .
COPY --from=frontend /app/web/dist /build/server/src/Entrypoints/Seacraft.Web/wwwroot

RUN dotnet restore ./server/src/Entrypoints/Seacraft.Web/Seacraft.Web.csproj --configfile ./.nuget/NuGet.config

WORKDIR /build/server/src/Entrypoints/Seacraft.Web/
RUN dotnet build Seacraft.Web.csproj.csproj  -nowarn:cs1591 -c Release

# =====================================================
FROM build AS publish

RUN dotnet publish Seacraft.Web.csproj.csproj -c Release -o /app

# =====================================================
FROM base AS final

WORKDIR /app

COPY --from=publish /app .
COPY --from=build /build/utility/run.sh ./run.sh

ENV ASPNETCORE_URLS  http://*:5000
ENV TZ Asia/Shanghai

ENTRYPOINT ["bash", "run.sh"]
