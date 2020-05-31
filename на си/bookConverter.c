#include <stdio.h>
#include <stdint.h>
#include <time.h>
#include <stdlib.h>
#include <string.h>

#include <wctype.h>
#include <locale.h>

#include <sys/time.h>

// для красивости
typedef int utf32;

// <сплагиаченный код для парсинга utf8>
#define UTF8_ACCEPT 0
#define UTF8_REJECT 12

const uint8_t utf8d[] = {
  // The first part of the table maps bytes to character classes that
  // to reduce the size of the transition table and create bitmasks.
   0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,  0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
   0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,  0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
   0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,  0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
   0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,  0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
   1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,  9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,
   7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,  7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,
   8,8,2,2,2,2,2,2,2,2,2,2,2,2,2,2,  2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,
  10,3,3,3,3,3,3,3,3,3,3,3,3,4,3,3, 11,6,6,6,5,8,8,8,8,8,8,8,8,8,8,8,

  // The second part is a transition table that maps a combination
  // of a state of the automaton and a character class to a state.
   0,12,24,36,60,96,84,12,12,12,48,72, 12,12,12,12,12,12,12,12,12,12,12,12,
  12, 0,12,12,12,12,12, 0,12, 0,12,12, 12,24,12,12,12,12,12,24,12,24,12,12,
  12,12,12,12,12,12,12,24,12,12,12,12, 12,24,12,12,12,12,12,12,12,24,12,12,
  12,12,12,12,12,12,12,36,12,36,12,12, 12,36,12,12,12,12,12,36,12,36,12,12,
  12,36,12,12,12,12,12,12,12,12,12,12,
};

int decode(int* state, utf32* codep, uint8_t byte){
  int type = utf8d[byte];

  *codep = (*state != UTF8_ACCEPT) ?
    (byte & 0x3fu) | (*codep << 6) :
    (0xff >> type) & (byte);

  *state = utf8d[256 + *state + type];
  return *state;
}
// </сплагиаченный код для парсинга utf8>

char* table[] = {
  ['A'] = "А",
  ['B'] = "Б",
  ['C'] = "Ц",
  ['D'] = "Д",
  ['E'] = "Е",
  ['F'] = "Ф",
  ['G'] = "Г",
  ['H'] = "Х",
  ['I'] = "И",
  ['J'] = "Ж",
  ['K'] = "К",
  ['L'] = "Л",
  ['M'] = "М",
  ['N'] = "Н",
  ['O'] = "О",
  ['P'] = "П",
  ['Q'] = "КУ",
  ['R'] = "Р",
  ['S'] = "С",
  ['T'] = "Т",
  ['U'] = "У",
  ['V'] = "В",
  ['W'] = "У",
  ['X'] = "КС",
  ['Y'] = "Ы",
  ['Z'] = "З",
  ['a'] = "а",
  ['b'] = "б",
  ['c'] = "ц",
  ['d'] = "д",
  ['e'] = "е",
  ['f'] = "ф",
  ['g'] = "г",
  ['h'] = "х",
  ['i'] = "и",
  ['j'] = "ж",
  ['k'] = "к",
  ['l'] = "л",
  ['m'] = "м",
  ['n'] = "н",
  ['o'] = "о",
  ['p'] = "п",
  ['q'] = "ку",
  ['r'] = "р",
  ['s'] = "с",
  ['t'] = "т",
  ['u'] = "у",
  ['v'] = "в",
  ['w'] = "у",
  ['x'] = "кс",
  ['y'] = "ы",
  ['z'] = "з",
};

char unicodeBuf[10];

utf32 getUnicodeChar(FILE* file){
  utf32 utf8codepoint;
  int utf8state = UTF8_ACCEPT;

  int t;
  int i = 0;
  while((t = getc(file)) != EOF){
    unicodeBuf[i++] = t;
    decode(&utf8state, &utf8codepoint, t);
    if(utf8state == UTF8_REJECT)break;
    if(utf8state == UTF8_ACCEPT){
      unicodeBuf[i++] = '\0';
      return utf8codepoint;
    }
  }
  if(utf8state != UTF8_ACCEPT){
    fprintf(stderr, "У нас всё плохо с utf8\n");
    exit(1);
  }
  return -1;
}

int time_ms(){
  struct timeval stop;
  gettimeofday(&stop,NULL);
  return stop.tv_usec/1000+stop.tv_sec*1000;
}

int main(int argc, char** argv){
  if(argc != 3){
    fprintf(stderr, "У нас всё плохо с аргументами (надо `%s from.txt to.txt`)\n", argv[0]);
    exit(1);
  }

  FILE* in = fopen(argv[1], "r");
  FILE* out = fopen(argv[2], "w");

  if(!in || !out){
    fprintf(stderr, "У нас всё плохо с файлами\n");
    exit(1);
  }

  setlocale(LC_ALL, "");

  int inCount = 0;
  int outCount = 0;

  int startTime = time_ms();
  utf32 rune;
  while((rune = getUnicodeChar(in)) != EOF){
    inCount++;
    if(rune < 0x80 && table[rune]){
      char* str = table[rune];
      outCount++;
      // это прям ужасный костыль но зато он быстрый
      if(str[2]){
        outCount++;
        putc(str[0], out);
        putc(str[1], out);
        putc(str[2], out);
        putc(str[3], out);
      }else{
        putc(str[0], out);
        putc(str[1], out);
      }
    }
    else if(!iswalpha(rune)){
      fputs(unicodeBuf, out);
      outCount++;
    }
  }
  int endTime = time_ms();

  printf("Файл \x1b[32m%*s\x1b[0m│ %*d → %*d (\x1b[35m%0.3f\x1b[0m sec)\n",
    -18, argv[1],
    7, inCount,
    7, outCount,
    (endTime - startTime) / 1e3
  );

  fclose(in);
  fclose(out);
}
