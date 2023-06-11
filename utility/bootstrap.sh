#!/bin/bash

set -x

npm -g i pnpm@6.32.15 husky

cd web || {
    echo "Failed to find the web project."
    exit 1
}

pnpm install
