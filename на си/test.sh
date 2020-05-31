#!/bin/sh

TIMEFORMAT=$'Всего \x1b[36m%R\x1b[0m секунд'
time ./part1.sh
time ./part2.sh
./bookConverter "book_from_web.txt" "new_book_from_web.txt"
