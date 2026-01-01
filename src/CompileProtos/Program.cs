using System;
using System.Diagnostics;
using System.IO;

namespace CompileProtos
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                var typeFormat = args[0];
                Console.WriteLine($"Compiling with protoc as {typeFormat}");

                foreach(var file in Directory.GetFiles(Environment.CurrentDirectory, "*.proto", SearchOption.AllDirectories))
                {
                    var protoPath = Path.GetFullPath(file).Replace(Path.GetFileName(file), "");
                    Console.WriteLine("Compiling " + Path.GetFileName(file));
                    var p = Process.Start("protoc", $"--proto_path={protoPath} --{typeFormat}={Path.Combine(Environment.CurrentDirectory, "compiled")} {file}");
                    p.WaitForExit();
                }
                Console.WriteLine("All done, press any key to continue...");
            }
            else
            {
                Console.WriteLine("Invalid arguments specified: " + string.Join(';', args));
            }
            Console.Read();
        }
    }
}
