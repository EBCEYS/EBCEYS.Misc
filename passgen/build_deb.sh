#!/bin/bash
rm -rf ./pgen/usr/bin && \
./build.sh && \
mkdir ./pgen/usr/bin && \
cp ./builded/* ./pgen/usr/bin && \
dpkg-deb --build pgen