using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookConverter
{
    // парадигма слизана с node.js
    // https://developer.mozilla.org/en-US/docs/Web/API/Console/time
    static class ConsoleTime
    {
        static private Dictionary<string, Stopwatch> timers = new Dictionary<string, Stopwatch>();
        const string defaultName = @"¯\_(ツ)_/¯";

        public static void Start(string name = defaultName)
        {
            if (timers.ContainsKey(name)) throw new Exception("такой таймер у нас уже есть");
            timers[name] = new Stopwatch();
            timers[name].Start();
        }

        public static void End(string name = defaultName)
        {
            if (!timers.ContainsKey(name)) throw new Exception("такого таймера у нас ещё нет");
            timers[name].Stop();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"{name}: {timers[name].Elapsed} ({timers[name].ElapsedMilliseconds / 1000.0} sec)");
            Console.ResetColor();
            timers.Remove(name);
        }
    }
}
