using System;
using System.Collections.Generic;
using static System.Console;
using static System.ConsoleColor;

namespace SmartCalc.Global.Common
{
    internal static class Global
    {
        public static ConsoleColor GetRandColor()
        {
            // default Console colors            
            List<ConsoleColor> colors = new List<ConsoleColor>() {
                    Black, DarkGray, DarkBlue, DarkGreen, DarkCyan,
                    DarkRed, DarkMagenta, DarkYellow, White, Gray,
                    Blue, Green, Cyan, Red, Magenta, Yellow
                                    };
                                    
            foreach (var color in colors)
            {
                if (color == BackgroundColor)
                    colors.Remove(color);
                break;
            }

            var random = new Random();
            var randomNumber = random.Next(0, colors.Count);
            var newColor = colors[randomNumber];

            return newColor;
        }

    }
}