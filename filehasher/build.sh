#!/bin/bash
rm -rf ./builded && \
dotnet publish ./filehasher/filehasher.csproj -c Release -r linux-x64 -o ./builded-fh && \
dotnet publish ./hashcomparer/hashcomparer.csproj -c Release -r linux-x64 -o ./builded-hc && \
rm -f ./builded-fh/*.dbg && \
rm -f ./builded-fh/*.pdb && \
rm -f ./builded-hc/*.dbg && \
rm -f ./builded-hc/*.pdb