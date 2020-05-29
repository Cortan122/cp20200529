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
    static class Program
    {
        /// <summary>
        /// Запускает MainTask() и предлогает пользователю повторить.
        /// </summary>
        static async Task Main()
        {
            while (true)
            {
                try
                {
                    await MainTask();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"поизашёл эксепшен: {e.Message}");
                    Console.ResetColor();
                }

                // if Console.IsOutputRedirected is True Console.ReadKey() throws an exeption
                if (Console.IsOutputRedirected) return;
                Console.WriteLine("Нажмите Enter чтобы повторить");
                if (Console.ReadKey(true).Key != ConsoleKey.Enter) break;
            }
        }

        static private async Task MainTask()
        {
            await Downloader.GetBooks();
            var files = Directory.EnumerateFiles("books", "*.txt", SearchOption.AllDirectories);
            files = files.Where(file => !Path.GetFileName(file).StartsWith("new_"));
            // Console.WriteLine(files.Count());

            ConsoleTime.Start("Part1");
            foreach (var file in files)
            {
                Converter.Koi7ifyFile(file);
            }
            ConsoleTime.End("Part1");

            ConsoleTime.Start("Part2");
            await Task.WhenAll(files.Select(file =>
              Task.Run(() => Converter.Koi7ifyFile(file))
            ));
            ConsoleTime.End("Part2");

            ConsoleTime.Start("Part3");
            await Downloader.GetEBook();
            Converter.Koi7ifyFile("book_from_web.txt");
            ConsoleTime.End("Part3");
        }
    }
}
