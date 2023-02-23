#!/bin/sh
path=`pwd`

echo "============================================"
cd $path
git pull -v
echo "============================================"
cd $path/Artfact/
git pull -v
echo "============================================"
cd $path/Assets/Projects/
git pull -v
echo "============================================"