using System;

namespace BeeGees_ReadNode
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Initialize("log_reader.txt", LogLevel.All, false);
            //new FastAccessTest().Test();

            var entryPoint = new ReaderEntryPoint();
            entryPoint.WaitForConnections();

            Console.Read();
        }
    }
}
