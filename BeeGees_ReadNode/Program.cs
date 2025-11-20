namespace BeeGees_ReadNode
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Initialize("log_reader.txt", LogLevel.All, false);

            var entryPoint = new ReaderEntryPoint();
            await entryPoint.WaitForConnectionsAsync();

            Console.Read();
        }
    }
}
