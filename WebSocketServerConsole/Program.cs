using Serilog;

namespace WebSocketServerConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();


            Server server = new();
            server.Start();
        }
    }
}
