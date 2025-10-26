using System.Net;
using System.Net.Sockets;
using Serilog;

namespace WebSocketServerConsole
{
    public class Server
    {
        private readonly Socket _socket;
        private readonly IPEndPoint _iPEndPoint;
        private bool isRunning = false;

        public Server()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _iPEndPoint = new IPEndPoint(IPAddress.Loopback, 80);
        }
        public Server(string ipAddress, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress? parsedIpAddress = ParseIpAddress(ipAddress);
            if (parsedIpAddress == null)
            {
                Log.Fatal("Wrong IP address.");
                Environment.Exit(1);
            }
            _iPEndPoint = new IPEndPoint(parsedIpAddress, port);
        }

        public async void Start()
        {
            isRunning = true;

            while (isRunning)
            {
                _socket.Bind(_iPEndPoint);
                _socket.Listen();
                Log.Information("Server started.");
                Log.Information("Listening on IP address: " + _iPEndPoint.Address.ToString() + ":" + _iPEndPoint.Port);

                Socket request = await _socket.AcceptAsync();
            }
            Stop();
        }

        public void Stop()
        {
            isRunning = false;
            _socket.Shutdown(SocketShutdown.Both);
        }
        private IPAddress? ParseIpAddress(string ipAddress)
        {
            IPAddress? result;
            IPAddress.TryParse(ipAddress, out result);
            return result;
        }
    }
}
