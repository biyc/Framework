#!/bin/sh
path=`pwd`

echo "============================================"
cd $path
git checkout master
echo "============================================"
cd $path/Artfact/
git checkout master
echo "============================================"
cd $path/Assets/Projects/
git checkout master
echo "============================================"