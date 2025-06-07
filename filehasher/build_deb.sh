#!/bin/bash
rm -rf ./fhasher/usr/bin && \
./build.sh && \
mkdir -p ./fhasher/usr/bin && \
mkdir -p ./hashcomp/usr/bin && \
cp ./builded-fh/* ./fhasher/usr/bin && \
cp ./builded-hc/* ./hashcomp/usr/bin && \
dpkg-deb --build fhasher && \
dpkg-deb --build hashcomp