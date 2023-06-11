#!/bin/bash

set -x

cd web || {
    echo "Failed to find the web project."
    exit 1
}

npm run build
