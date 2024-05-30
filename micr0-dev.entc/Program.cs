using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Irony.Parsing;

namespace Rottytooth.Entropy
{
    class Program
    {
        private const int INVALID_PARAMETER = -1;
        private static bool _debugMode = false;

        public static int Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "help" || args[0] == "--help" || args[0] == "-h")
            {
                PrintHelp();
                return 0;
            }

            string command = args[0].ToLower();
            string inputPath = null;
            float mutationRate = Real.MutationRate;

            // Parse arguments for options
            for (int i = 1; i < args.Length; i++)
            {
                if (args[i].StartsWith("--mutation-rate=") || args[i].StartsWith("-m="))
                {
                    if (!float.TryParse(args[i].Split('=')[1], out mutationRate) ||
                        mutationRate < Real.MinMutation || mutationRate > Real.MaxMutation)
                    {
                        Console.WriteLine("Invalid mutation rate provided.");
                        PrintHelp();
                        return INVALID_PARAMETER;
                    }
                }
                else if (!args[i].StartsWith("-") && inputPath == null)
                {
                    inputPath = args[i];
                }
                else
                {
                    Console.WriteLine($"Invalid option: {args[i]}");
                    PrintHelp();
                    return INVALID_PARAMETER;
                }
            }

            // Set Mutation Rate
            Rottytooth.Entropy.Real.MutationRate = mutationRate;

            // Execute command
            switch (command)
            {
                case "run":
                    return Run(inputPath);
                case "compile":
                    return Compile(inputPath);
                default:
                    Console.WriteLine($"Invalid command: {command}");
                    PrintHelp();
                    return INVALID_PARAMETER;
            }
        }

        static int Run(string inputPath)
        {
            if (string.IsNullOrEmpty(inputPath))
            {
                Console.WriteLine("No input file specified.");
                return INVALID_PARAMETER;
            }
            string program = GenerateCode(inputPath);
            if (program == null) return INVALID_PARAMETER;

            if (_debugMode)
            {
                Console.WriteLine(program);
                return 0;
            }
            else
            {
                CodeGenerator.Execute(program);
            }
            return 0;
        }

        static int Compile(string inputPath)
        {
            if (string.IsNullOrEmpty(inputPath))
            {
                Console.WriteLine("No input file specified.");
                return INVALID_PARAMETER;
            }

            string program = GenerateCode(inputPath);
            if (program == null) return INVALID_PARAMETER;
            Console.WriteLine(program); // Output the generated C# code
            return 0;
        }

        static string GenerateCode(string inputPath)
        {
            EntropyGrammar grammar = new EntropyGrammar();
            Parser parser = new Parser(grammar);

            try
            {
                TextReader reader = new StreamReader(inputPath);
                System.IO.FileInfo file = new FileInfo(inputPath);
                string fileName = Path.GetFileNameWithoutExtension(file.FullName);
                ParseTree programTree = parser.Parse(reader.ReadToEnd());

                if (CheckForErrors(programTree, parser))
                {
                    return null;
                }

                CodeConverter converter = new CodeConverter(programTree);
                return converter.ToCSharp();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.GetType() + " error thrown: " + ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
                return null;
            }
        }

        static bool CheckForErrors(ParseTree programTree, Parser parser)
        {
            if (programTree == null || parser.Context.HasErrors)
            {
                Console.Error.WriteLine("An error has occurred");
                if (parser.Context.CurrentParserInput != null)
                {
                    Console.Error.WriteLine(parser.Context.CurrentParserInput.Token.ToString());
                    Console.Error.WriteLine("Line #" + parser.Context.CurrentParserInput.Token.Location.Line);
                    Console.Error.WriteLine("Character #" + parser.Context.CurrentParserInput.Token.Location.Column);
                    Console.Error.WriteLine("Failed at: " + parser.Context.Source.Text.Substring(
                        parser.Context.CurrentParserInput.Token.Location.Position,
                        (parser.Context.Source.Text.Length - parser.Context.CurrentParserInput.Token.Location.Position > 20) ? 20 : parser.Context.Source.Text.Length - parser.Context.CurrentParserInput.Token.Location.Position)
                        + "...");
                }
                return true;
            }
            return false;
        }

        static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Entropy Compiler - entc");
            Console.WriteLine("Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  entc [command] [options] <input_file.en>");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("  run      Translates the input file and runs it.");
            Console.WriteLine("  compile  Translates the input file to C# and prints the output.");
            Console.WriteLine("  help     Displays this help message.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -m=<value>, --mutation-rate=<value>  Sets the mutation rate (default: 2).");
            Console.WriteLine("                                     Value should be between " +
                              $"{Real.MinMutation} and {Real.MaxMutation}.");
            Console.WriteLine();
            Console.WriteLine("Example:");
            Console.WriteLine("  entc run myprogram.en --mutation-rate=0.5");
        }
    }
}
