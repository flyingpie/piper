#!/usr/bin/env bash
set -euo pipefail

cd "$(dirname "$0")"

# The hot reload server, run directly from its source checkout (the same one Piper.Host.Photino references in Debug builds).
DTR_HOTRELOAD_SOURCE="${DTR_HOTRELOAD_SOURCE:-$(cd ../../../sandbox/hotreload/blazor-hotreload-review/src && pwd)}"
PIPER_PROJECT="$(pwd)/40-Host/Piper.Host.Photino/Piper.Host.Photino.csproj"

dotnet build "$PIPER_PROJECT" -c Debug -p:DtrHotReloadSource="$DTR_HOTRELOAD_SOURCE"

dotnet run --project "$DTR_HOTRELOAD_SOURCE/Dtr.Blazor.HotReload.Server/Dtr.Blazor.HotReload.Server.csproj" -c Debug -- \
	--project "$PIPER_PROJECT" \
	--devserver \
	--launch-profile Run \
	"$@"
