using Serilog;

namespace WebSocketServerConsole
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();


            Server server = new();
            await server.Start();
        }
    }
}
