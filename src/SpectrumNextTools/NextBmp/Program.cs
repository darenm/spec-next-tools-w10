using SpectrumNextTools.Library;
using SpectrumNextTools.Library.Images.Bitmaps;
using System;
using System.Linq;

namespace NextBmp
{
    class Program
    {
        const int ErrorCode = -1;
        const int OkCode = 0;

        static int Main(string[] args)
        {
            bool showHeader = false;
            NextBmpConverter converter = null;


            if (!args.Any())
            {
                PrintUsage();
                return ErrorCode;
            }

            var options = new BitmapOptions();
            foreach (var arg in args)
            {
                if (arg.StartsWith('-'))
                {
                    switch (arg)
                    {
                        case "-floor":
                            if (options.RoundingMode == RoundingMode.Unknown)
                            {
                                options.RoundingMode = RoundingMode.Floor;
                            }
                            else
                            {
                                WriteError($"Invalid option: {arg}");
                                PrintUsage();
                                return ErrorCode;
                            }
                            break;

                        case "-ceil":
                            if (options.RoundingMode == RoundingMode.Unknown)
                            {
                                options.RoundingMode = RoundingMode.Ceil;
                            }
                            else
                            {
                                WriteError($"Invalid option: {arg}");
                                PrintUsage();
                                return ErrorCode;
                            }

                            break;

                        case "-round":
                            if (options.RoundingMode == RoundingMode.Unknown)
                            {
                                options.RoundingMode = RoundingMode.Round;
                            }
                            else
                            {
                                WriteError($"Invalid option: {arg}");
                                PrintUsage();
                                return ErrorCode;
                            }

                            break;

                        case "-min-palette":
                            {
                                options.MinimizePalette = true;

                                break;
                            }

                        case "-std-palette":
                            {
                                options.UseStdPalette = true;

                                break;
                            }

                        case "-help":
                            {
                                PrintUsage();
                                return ErrorCode;
                            }

                        case "-show-header":
                            {
                                showHeader = true;
                                break;
                            }

                        default:
                            WriteError($"Invalid option: {arg}");
                            PrintUsage();
                            return ErrorCode;
                    }
                }
                else
                {
                    if (options.InFileName == null)
                    {
                        options.InFileName = arg;
                    }
                    else if (options.OutFileName == null)
                    {
                        options.OutFileName = arg;
                    }
                    else
                    {
                        WriteError("Too many arguments");
                        PrintUsage();
                        return ErrorCode;
                    }
                }
            }

            try
            {
                converter = new NextBmpConverter(options);
                converter.ProcessImage();
            }
            catch (Exception ex)
            {
                WriteError($"Error: {ex.Message}");
                PrintUsage();
                return ErrorCode;
            }
            finally
            {
                if (showHeader && converter != null)
                {
                    Console.WriteLine($"Header Type:                       {System.Text.Encoding.Default.GetString(converter.InBitmap.Header.Type)}");
                    Console.WriteLine($"Header FileSize:                   {converter.InBitmap.Header.FileSize}");
                    Console.WriteLine($"Header Reserved1:                  {converter.InBitmap.Header.Reserved1}");
                    Console.WriteLine($"Header Reserved2:                  {converter.InBitmap.Header.Reserved2}");
                    Console.WriteLine($"Header ImageDataOffset:            {converter.InBitmap.Header.ImageDataOffset}");
                    Console.WriteLine($"InformationHeader Planes:          {converter.InBitmap.InformationHeader.Planes}");
                    Console.WriteLine($"InformationHeader BitsPerPixel:    {converter.InBitmap.InformationHeader.BitsPerPixel}");
                    Console.WriteLine($"InformationHeader CompressionType: {converter.InBitmap.InformationHeader.CompressionType}");
                    Console.WriteLine($"InformationHeader ImageSize:       {converter.InBitmap.InformationHeader.ImageSize}");
                    Console.WriteLine($"InformationHeader XResolution:     {converter.InBitmap.InformationHeader.XResolution}");
                    Console.WriteLine($"InformationHeader YResolution:     {converter.InBitmap.InformationHeader.YResolution}");
                    Console.WriteLine($"InformationHeader NumberOfColors:  {converter.InBitmap.InformationHeader.NumberOfColors}");
                    Console.WriteLine($"InformationHeader ImportantColors: {converter.InBitmap.InformationHeader.ImportantColors}");
                }

            }

            return OkCode;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: nextbmp [-floor|-ceil|-round] [-min-palette] [-std-palette] <srcfile.bmp> [<dstfile.bmp>]");
            Console.WriteLine("Convert the palette in an uncompressed 8-bit BMP file to Sinclair ZX Spectrum Next format.");
            Console.WriteLine("If no destination BMP file is specified, the source BMP file is modified.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -floor        Round down the color values to the nearest integer.");
            Console.WriteLine("  -ceil         Round up the color values to the nearest integer.");
            Console.WriteLine("  -round        Round the color values to the nearest integer (default).");
            Console.WriteLine("  -min-palette  If specified, minimize the palette by removing any duplicated colors, sort");
            Console.WriteLine("                it in ascending order, and clear any unused palette entries at the end.");
            Console.WriteLine("                This option is ignored if the -std-palette option is given.");
            Console.WriteLine("  -std-palette  If specified, convert to the Spectrum Next standard palette colors.");
            Console.WriteLine("  -show-header  If specified, display the header details.");
            Console.WriteLine("  -help         Display this help content.");
        }

        private static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
            Console.ResetColor();
        }
    }
}
