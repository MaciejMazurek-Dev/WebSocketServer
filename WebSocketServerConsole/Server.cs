using System.Net;
using System.Net.Sockets;
using System.Text;
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

        public async Task Start()
        {
            _socket.Bind(_iPEndPoint);
            _socket.Listen();
            try
            {
                isRunning = true;

                Log.Information("Server started.");
                Log.Information("Listening on IP address: " + _iPEndPoint.Address.ToString() + ":" + _iPEndPoint.Port);
                Socket requestHandler = await _socket.AcceptAsync();
                while (isRunning)
                {
                    byte[] buffer = new byte[1024];
                    int bytesReceived = await requestHandler.ReceiveAsync(buffer);

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    Log.Information("Message received: " + message);
                }
            }
            catch(Exception ex)
            {
                Log.Warning("Exception: " + ex.Message);
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
