using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace EXSearch {
    class Program {

        static string Log = null;
        static string Filter = "*.*";
        static void Main(string[] args) {
            Console.Title = "Encoding X Searcher - Encoding Discovery Tool";
            string Text, Path;
            ParseArgs(args, out Text, out Path);

            string[] Files = PathWork(Path);

            StringBuilder SB = new StringBuilder();
            SB.AppendLine("Encoding X Search - Result List");

            foreach (string File in Files) {
                Console.WriteLine($"Searching inside the \"{System.IO.Path.GetFileName(File)}\"");
                Stream Reader = new StreamReader(File).BaseStream;
                byte[] Buffer = new byte[Text.Length * 2];
                long Ptr = Reader.Position;
                while (Reader.Read(Buffer, 0, Buffer.Length) > 0) {
                    Reader.Position = ++Ptr;
                    if (TestAt(Buffer, Text) || TestAtUni(Buffer, Text)) {
                        Console.WriteLine($"Match At 0x{Ptr - 1:X8}");
                        SB.AppendLine($"Match at 0x{Ptr - 1:X8} in the {System.IO.Path.GetFileName(File)}");
                    }
                }
                Reader.Close();
            }

            if (Log != null) {
                Console.WriteLine("Generating Log File...");
                File.WriteAllText(Log, SB.ToString());
            }

            Console.WriteLine("Press a Key To Exit...");
            Console.ReadKey();
            Console.WriteLine();
        }
        private static bool TestAt(byte[] Data,string Text) {
            Dictionary<char, ushort> CharMap = new Dictionary<char, ushort>();
            try {
                for (int i = 0; i < Text.Length; i++) {
                    byte b = Data[i];
                    char c = Text[i];
                    if (CharMap.ContainsKey(c)) {
                        if (b != CharMap[c])
                            return false;
                    } else {
                        if (CharMap.ContainsValue(b))
                            return false;
                        CharMap.Add(c, b);
                    }
                }
            } catch {
                return false;
            }

            return true;
        }
        private static bool TestAtUni(byte[] Data, string Text) {
            Dictionary<char, ushort> CharMap = new Dictionary<char, ushort>();

            try {
                for (int i = 0; i < Text.Length; i++) {
                    ushort b = BitConverter.ToUInt16(new byte[] { Data[(i*2) + 1], Data[i*2] }, 0);
                    char c = Text[i];
                    if (CharMap.ContainsKey(c)) {
                        if (b != CharMap[c])
                            return false;
                    } else {
                        if (CharMap.ContainsValue(b))
                            return false;
                        CharMap.Add(c, b);
                    }
                }
            } catch {
                return false;
            }

            return true;
        }
        
        private static string[] PathWork(string path) {
            if (File.Exists(path))
                return new string[] { path };

            if (Directory.Exists(path))
                return Directory.GetFiles(path, Filter, SearchOption.AllDirectories);

            throw new FileNotFoundException("File/Directory not found", path);
        }

        private static void ParseArgs(string[] Args, out string Text, out string Path) {
            Text = string.Empty;
            Path = string.Empty;

            for (int i = 0; i < Args.Length; i++) {
                string Arg = Args[i].TrimStart('-', '\\', '/');
                switch (Arg.ToLower()) {
                    case "t":
                    case "text":
                    case "line":
                        Text = Args[++i];
                        break;
                    case "p":
                    case "path":
                    case "directory":
                    case "file":
                    case "inside":
                        Path = Args[++i];
                        break;
                    case "f":
                    case "filter":
                    case "extension":
                    case "ext":
                        Filter = Args[++i];
                        break;
                    case "l":
                    case "log":
                        Log = Args[++i];
                        break;
                    case "about":
                    case "help":
                    case "?":
                        Console.WriteLine("EXSearch - Encoding Discovery Tool");
                        Console.WriteLine("This tool can search text using an unknown encoding, for it to work you need to use a line that contains repetitions of a same letter in a non sequencial form, the more repetitions you have more precise will be the result, the tool even if not supporting cryptography can too be used to search text in \"encrypted\" files by a simple XOR of one byte");

                        Console.WriteLine();

                        Console.WriteLine("Usage:");
                        Console.WriteLine("\tEXSearch -t \"Sample compatible text\" -p \"C:\\FilesDir\\\" -f \"*.bin\" -l \"Results.log\"");

                        Console.WriteLine("Press a Key to Exit");

                        Console.ReadKey();
                        Console.WriteLine();

                        Process.GetCurrentProcess().Kill();
                        break;
                }
            }


            if (string.IsNullOrWhiteSpace(Text)) {
                Console.WriteLine("Type a Text:");
                Text = Console.ReadLine();
            }

            if (string.IsNullOrWhiteSpace(Path)) {
                Console.WriteLine("Type a path to search:");
                Path = Console.ReadLine();
            }
        }
    }
}
