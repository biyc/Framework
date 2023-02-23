#!/bin/sh
path=`pwd`

echo "============================================"
cd $path
git checkout story
echo "============================================"
cd $path/Artfact/
git checkout story
echo "============================================"
cd $path/Assets/Projects/
git checkout story
echo "============================================"