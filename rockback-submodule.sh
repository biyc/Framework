#!/bin/sh
path=`pwd`

echo "============================================"
cd $path
git checkout .
git clean -df
echo "============================================"
cd $path/Artfact/
git checkout .
git clean -df
echo "============================================"
cd $path/Assets/Projects/
git checkout .
git clean -df
echo "============================================"