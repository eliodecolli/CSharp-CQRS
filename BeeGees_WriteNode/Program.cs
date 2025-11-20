namespace BeeGees_WriteNode
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Initialize("writer_log.txt", LogLevel.All, false);
            var entryPoint = new WriterEntryPoint(new Facade.Facade());

            await entryPoint.WaitForWorkAsync();
            Console.Read();
        }
    }
}
