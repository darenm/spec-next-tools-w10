using System;
using System.IO;
using SpectrumNextTools.Library;

namespace TgaReader
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("    tgareader <filename.tga>");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("    tgareader <filename.tga>");
                Console.WriteLine();
                Console.WriteLine($"\"{args[0]}\" not found.");
                return -1;
            }

            var tgaImage = new TgaImage(args[0]);
            tgaImage.Validate();

            Console.WriteLine($"TGA Info: {args[0]}");
            Console.WriteLine();
            Console.WriteLine($"  ColorMapLength:    {tgaImage.Header.ColorMapSpecification.ColorMapLength}");
            Console.WriteLine($"  ColorMapEntrySize: {tgaImage.Header.ColorMapSpecification.ColorMapEntrySize}");
            Console.WriteLine($"  XOrigin:           {tgaImage.Header.ImageSpecification.XOrigin}");
            Console.WriteLine($"  YOrigin:           {tgaImage.Header.ImageSpecification.YOrigin}");
            Console.WriteLine($"  ImageWidth:        {tgaImage.Header.ImageSpecification.ImageWidth}");
            Console.WriteLine($"  ImageHeight:       {tgaImage.Header.ImageSpecification.ImageHeight}");
            Console.WriteLine();
            Console.WriteLine("  Palette:");
            for (var i = 0; i < tgaImage.Header.ColorMapData.TgaColors.Length; i++)
            {
                var color = tgaImage.Header.ColorMapData.TgaColors[i];
                Console.WriteLine($"    {i:D3} - {color.Red:D3}, {color.Green:D3}, {color.Blue:D3}");
            }

            return 0;
        }
    }
}
