#!/bin/bash
rm -rf ./builded && \
dotnet publish ./passgen/passgen.csproj -c Release -r linux-x64 -o ./builded && \
rm -f ./builded/*.dbg