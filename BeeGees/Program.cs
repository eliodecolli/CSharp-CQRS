using System;
using System.IO;

namespace BeeGees_WriteNode
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Initialize("writer_log.txt", LogLevel.All, false);
            var entryPoint = new WriterEntryPoint(new Facade.Facade());

            entryPoint.WaitForWork();
            Console.Read();
        }
    }
}
