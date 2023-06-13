#!/bin/bash

set -x

install_nodejs() {
    local NVM_ARCH
    local NVM_MIRROR="https://nodejs.org/dist"
    local NVM_VERSION="v18.4.0"
    HOST_ARCH=$(dpkg --print-architecture)
    case "${HOST_ARCH}" in
    i*86)
        NVM_ARCH="x86"
        NVM_MIRROR="https://unofficial-builds.nodejs.org/download/release"
        ;;
    x86_64 | amd64)
        NVM_ARCH="x64"
        ;;
    aarch64 | armv8l)
        NVM_ARCH="arm64"
        ;;
    *)
        NVM_ARCH="${HOST_ARCH}"
        ;;
    esac

    local FILE_NAME="node-$NVM_VERSION-linux-$NVM_ARCH.tar.gz"
    local URL="$NVM_MIRROR/$NVM_VERSION/$FILE_NAME"

    wget "$URL"
    tar -C /usr/local --strip-components 1 -xzf "$FILE_NAME"
    rm "$FILE_NAME"
}

install_dotnet() {
    wget https://dot.net/v1/dotnet-install.sh
    chmod a+x dotnet-install.sh
    ./dotnet-install.sh --channel 7.0
    rm dotnet-install.sh
}

hash dotnet 2>/dev/null || {
    install_dotnet
}

hash node 2>/dev/null || {
    install_nodejs
}

hash pnpm 2>/dev/null || {
    npm -g i pnpm@6.32.15
}

cd web || {
    echo "Failed to find the web project."
    exit 1
}

pnpm install
