#!/usr/bin/env bash

# ./src/dev-desktop.sh "$@"

command dotnet tool restore
command dotnet build 40-Host/Piper.Host.Photino/Piper.Host.Photino.csproj -c Debug
command dotnet tool run \
	blazor-hmr \
	--project 40-Host/Piper.Host.Photino/Piper.Host.Photino.csproj \
	--devserver \
	--launch-profile Run \
	"$@"
