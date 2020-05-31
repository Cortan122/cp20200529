all: bookConverter

bookConverter: bookConverter.c
	gcc -O3 bookConverter.c -o bookConverter

clean:
	rm -f bookConverter *.txt *.zip
	rm -rf Книги

book_from_web.txt:
	./getFiles.sh

run: bookConverter book_from_web.txt
	./test.sh
