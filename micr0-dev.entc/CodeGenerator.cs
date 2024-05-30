using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace Rottytooth.Entropy
{
    internal class CodeGenerator
    {
        internal static void Execute(string cSharpCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(cSharpCode);

            // Assuming Rottytooth.Entropy.dll is in the same directory as this assembly
            string currentAssemblyPath = Assembly.GetExecutingAssembly().Location;
            string entropyDllPath = Path.Combine(Path.GetDirectoryName(currentAssemblyPath), "Rottytooth.Entropy.dll");

            // Add references
            var references = new[] {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(entropyDllPath),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
            };

            // Compile the code
            var compilation = CSharpCompilation.Create("DynamicAssembly",
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            // Emit the compiled code 
            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);

                    // Load the assembly from the memory stream
                    Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
                    MethodInfo mainMethod = assembly.EntryPoint;

                    if (mainMethod != null)
                    {
                        object[] parametersArray = new object[] { };
                        mainMethod.Invoke(null, parametersArray);
                    }
                    else
                    {
                        Console.WriteLine("No suitable entry point found in the compiled assembly.");
                    }
                }
                else
                {
                    // Handle compilation errors
                    foreach (Diagnostic diagnostic in result.Diagnostics)
                    {
                        Console.Error.WriteLine(diagnostic.ToString());
                    }
                }
            }
        }
    }
}
