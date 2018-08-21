using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace WebIm.Utils
{
    public static class Extensions
    {
        public static void ConsoleWrite(this Image<Rgba32> img)
        {
            const int max = 28;
            var w = img.Width;
            var h = img.Height;

            // we need to scale down high resolution images...
            var complexity = (int)Math.Floor(Convert.ToDecimal(((w / max) + (h / max)) / 2));

            if (complexity < 1) { complexity = 1; }

            for (var x = 0; x < w; x += complexity)
            {
                for (var y = 0; y < h; y += complexity)
                {
                    var clr = img[x, y];
                    Console.ForegroundColor = GetNearestConsoleColor(clr);
                    Console.Write("█");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static ConsoleColor GetNearestConsoleColor(Rgba32 color)
        {
            // this is very likely to be awful and hilarious
            var r = color.R;
            var g = color.G;
            var b = color.B;
            var total = r + g + b;
            var darkThreshold = 0.35m; // how dark a color has to be overall to be the dark version of a color

            var cons = ConsoleColor.White;

            if (total >= 39 && total < 100 && AreClose(r, g) && AreClose(g, b) && AreClose(r, b))
            {
                cons = ConsoleColor.DarkGray;
            }

            if (total >= 100 && total < 180 && AreClose(r, g) && AreClose(g, b) && AreClose(r, b))
            {
                cons = ConsoleColor.Gray;
            }


            // if green is the highest value
            if (g > b && g > r)
            {
                // ..and color is less that 25% of color
                if (Convert.ToDecimal(total / 765m) < darkThreshold)
                {
                    cons = ConsoleColor.DarkGreen;
                }
                else
                {
                    cons = ConsoleColor.Green;
                }
            }

            // if red is the highest value
            if (r > g && r > b)
            {

                // ..and color is less that 25% of color
                if (Convert.ToDecimal(total / 765m) < darkThreshold)
                {
                    cons = ConsoleColor.DarkRed;
                }
                else
                {
                    cons = ConsoleColor.Red;
                }
            }

            // if blue is the highest value
            if (b > g && b > r)
            {
                // ..and color is less that 25% of color
                if (Convert.ToDecimal(total / 765m) < darkThreshold)
                {
                    cons = ConsoleColor.DarkBlue;
                }
                else
                {
                    cons = ConsoleColor.Blue;
                }
            }


            if (r > b && g > b && AreClose(r, g))
            {
                // ..and color is less that 25% of color
                if (Convert.ToDecimal(total / 765m) < darkThreshold)
                {
                    cons = ConsoleColor.DarkYellow;
                }
                else
                {
                    cons = ConsoleColor.Yellow;
                }
            }

            if (b > r && g > r && AreClose(b, g))
            {
                // ..and color is less that 25% of color
                if (Convert.ToDecimal(total / 765m) < darkThreshold)
                {
                    cons = ConsoleColor.DarkCyan;
                }
                else
                {
                    cons = ConsoleColor.Cyan;
                }
            }

            if (r > g && b > g && AreClose(r, b))
            {
                // ..and color is less that 25% of color
                if (Convert.ToDecimal(total / 765m) < darkThreshold)
                {
                    cons = ConsoleColor.DarkMagenta;
                }
                else
                {
                    cons = ConsoleColor.Magenta;
                }
            }

            if (total >= 180 && AreClose(r, g) && AreClose(g, b) && AreClose(r, b))
            {
                cons = ConsoleColor.White;
            }

            // BLACK
            if (total < 39)
            {
                cons = ConsoleColor.Black;
            }

            return cons;
        }

        /// <summary>
        /// Returns true if the numbers are pretty close
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool AreClose(int a, int b)
        {
            var diff = Math.Abs(a - b);
            return diff < 30;
        }
    }
}
