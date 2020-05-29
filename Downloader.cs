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
    static class Downloader
    {
        // exactly like `curl $url -o $path`
        // and https://docs.microsoft.com/en-us/dotnet/api/system.net.webclient.downloadfileasync?view=netcore-3.1
        // (я уже написал небуду переписывать)
        static private async Task Download(string url, string path)
        {
            var client = new HttpClient();
            var res = await client.GetAsync(url);
            if (!res.IsSuccessStatusCode) throw new Exception("чтото у нас вышла ошибочка " + res.StatusCode);
            await File.WriteAllBytesAsync(path, await res.Content.ReadAsByteArrayAsync());
        }

        static public async Task GetBooks()
        {
            if (Directory.Exists("books")) return;
            ConsoleTime.Start("GetBooks");
            await Download("https://getfile.dokpub.com/yandex/get/https://yadi.sk/d/fh3lefTcQ_hLLA", "books.zip");
            ZipFile.ExtractToDirectory("books.zip", "books");
            ConsoleTime.End("GetBooks");
        }

        static public async Task GetEBook()
        {
            if (File.Exists("book_from_web.txt")) return;
            ConsoleTime.Start("GetEBook");
            await Download("https://www.gutenberg.org/files/1342/1342-0.txt", "book_from_web.txt");
            ConsoleTime.End("GetEBook");
        }
    }
}
