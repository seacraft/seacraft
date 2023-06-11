#!/bin/bash

set -x

SCRIPTPATH="$(
    cd -- "$(dirname "$0")" >/dev/null 2>&1
    pwd -P
)"
ROOT_DIR="$(dirname "$SCRIPTPATH")"

WEB_DIST_DIR="$ROOT_DIR/web/dist/web"
ENTRY_PROJ_DIR="$ROOT_DIR/server/src/Entrypoints/Seacraft.Web"

bash "$SCRIPTPATH/build.web.sh"

cp -r "$WEB_DIST_DIR" "$ENTRY_PROJ_DIR/wwwroot"

cd "$ENTRY_PROJ_DIR" || echo "Failed to find the backend project."
dotnet restore

dotnet build
dotnet publish -c Release -o /app
