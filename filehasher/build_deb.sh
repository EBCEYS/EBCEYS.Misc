#!/bin/bash
rm -rf ./fhasher/usr/bin && \
./build.sh && \
mkdir ./fhasher/usr/bin && \
cp ./builded/* ./fhasher/usr/bin && \
dpkg-deb --build fhasher