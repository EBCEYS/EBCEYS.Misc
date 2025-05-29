#!/bin/bash
rm -rf ./builded && \
dotnet publish ./filehasher/filehasher.csproj -c Release -r linux-x64 -o ./builded && \
rm -f ./builded/*.dbg