#!/bin/sh

# тут надо -L чтобы он чтото делал при 302
curl -L 'https://getfile.dokpub.com/yandex/get/https://yadi.sk/d/fh3lefTcQ_hLLA' -o books.zip
curl 'https://www.gutenberg.org/files/1342/1342-0.txt' -o book_from_web.txt

unzip -o books.zip
