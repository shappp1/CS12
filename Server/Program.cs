using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server {
    class Program {
        async static void RunServer(int port) {
            TcpListener server = new TcpListener(new IPEndPoint(IPAddress.Any, port));

            List<TcpClient> clientList = new List<TcpClient>();

            while (true) {
                Task<TcpClient> client = server.AcceptTcpClientAsync();


            }
            
        }

        static void Main(string[] args) {
            RunServer(25565);
        }
    }
}
