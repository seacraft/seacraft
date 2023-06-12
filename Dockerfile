FROM node:14 AS frontend

RUN npm -g i pnpm@6.32.15 husky

WORKDIR /app

COPY ./web/package.json ./web/package.json

RUN cd ./web && pnpm install

COPY . .

RUN cd ./web/ && npm run build

# =====================================================
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

ARG TARGETPLATFORM
ARG TARGETARCH
ARG BUILDPLATFORM

RUN apt update && \
    apt install -y libgdiplus procps && \
    ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll && \
    apt clean

WORKDIR /docker

EXPOSE 5000

# =====================================================
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build

ARG TARGETPLATFORM
ARG TARGETARCH
ARG BUILDPLATFORM

WORKDIR /build

COPY ./src/Entrypoints/Seacraft/Seacraft.csproj ./src/Entrypoints/Seacraft/Seacraft.csproj

RUN dotnet restore ./src/Entrypoints/Seacraft/Seacraft.csproj --configfile ./.nuget/NuGet.Config -a $TARGETARCH

COPY . .
COPY --from=frontend /app/web/dist /build/engine/src/Entrypoints/Seacraft/wwwroot

WORKDIR /build/engine/src/Entrypoints/Seacraft/
RUN dotnet build Seacraft.csproj -nowarn:cs1591 -c Release -a $TARGETARCH

# =====================================================
FROM build AS publish

ARG TARGETPLATFORM
ARG TARGETARCH
ARG BUILDPLATFORM

RUN dotnet publish --no-restore Seacraft.csproj -c Release -o /app -a $TARGETARCH

# =====================================================
FROM base AS final

ARG TARGETPLATFORM
ENV TARGETPLATFORM=${TARGETPLATFORM:-linux/amd64}
ARG TARGETARCH
ARG BUILDPLATFORM

LABEL org.opencontainers.image.url="https://hub.docker.com/r/seacraft/seacraft/" \
      org.opencontainers.image.source="https://github.com/seacraft/seacraft" \      
      org.opencontainers.image.vendor="seacraft" \
      org.opencontainers.image.title="seacraft" \
      org.opencontainers.image.description="Dockerized seacraft" \
      org.opencontainers.image.authors="seacraft"

WORKDIR /app

COPY --from=publish /app .
COPY --from=build /build/scripts/run.sh ./run.sh

ENV ASPNETCORE_URLS  http://*:5000

ENTRYPOINT ["bash", "run.sh"]
