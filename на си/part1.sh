#!/bin/sh

cd Книги
for file in `ls *.txt | grep -v 'new_'`; do
  ../bookConverter "$file" "new_$file"
done
