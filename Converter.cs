using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookConverter
{
    static class Converter
    {
        static private readonly Dictionary<char, string> letterTable = new Dictionary<char, string>
        {
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
        };

        static Converter()
        {
            foreach (var pair in letterTable.ToArray())
            {
                letterTable[char.ToLower(pair.Key)] = pair.Value.ToLower();
            }
        }

        // это можно сделать раз в 100 эфективнее если использовать FileStream
        // сейчас тут память O(n) а можно сделать O(1)
        // но если надо чтобы быстро, то на си это прогу можно написать даже быстрее чем на с# (если знать си)
        // на си будет всё это быстрее например потомучто ему ненадо всё конвертитть между utf8 и utf16, тк он напрямую работает с utf8
        //
        // хотя вот:
        // я спецально написал такой медленный код, чтобы было более заметна разница между синхронным и асинхронным способом
        private static string Koi7ify(string text)
        {
            var str = new StringBuilder();
            foreach (var c in text)
            {
                if (!char.IsLetter(c))
                {
                    str.Append(c);
                    continue;
                }
                if (letterTable.ContainsKey(c)) str.Append(letterTable[c]);
            }
            return str.ToString();
        }

        public static void Koi7ifyFile(string path)
        {
            var str1 = File.ReadAllText(path);
            var str2 = Koi7ify(str1);
            var newFilename = Path.Combine(Path.GetDirectoryName(path), "new_" + Path.GetFileName(path));
            File.WriteAllText(newFilename, str2);
            Console.WriteLine($"{Path.GetFileName(path)}: {str1.Length} -> {str2.Length}");
        }
    }
}
