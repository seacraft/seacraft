#!/bin/bash

set -x

dotnet ef --project ../../src/Core/Seacraft.Repositories/ migrations add "$@"
