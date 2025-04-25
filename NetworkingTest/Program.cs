using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace NetworkingTest {
    class Program {
        static void Main(string[] args) {
            const int PORT = 25565;
            const string IP = "10.1.3.92";
            const string USERNAME = "shappp1";

            TcpClient client = new TcpClient();
            client.Connect(IP, PORT);
            NetworkStream stream = client.GetStream();

            string message;
            byte[] data;
            while (true) {
                Console.Write(": ");
                message = Console.ReadLine();

                if (message == "::QUIT") {
                    data = Encoding.UTF8.GetBytes("!!CDCON");
                    stream.Write(data, 0, data.Length);
                    break;
                }

                data = Encoding.UTF8.GetBytes($"[{USERNAME}]: " + message);
                stream.Write(data, 0, data.Length);

                data = new byte[0x400];
                int whatever = stream.Read(data, 0, 0x400);
                Console.WriteLine(Encoding.UTF8.GetString(data, 0, whatever));
            }

            stream.Close();
            client.Close();
        }
    }
}
