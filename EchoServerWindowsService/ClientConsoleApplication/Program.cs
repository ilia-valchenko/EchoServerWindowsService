using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ClientConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SendMessageFromSocket(11000);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Error: {exc.Message}");
            }
            finally
            {
                Console.WriteLine("\nTap to continue...");
                Console.ReadKey(true);
            }
        }

        static void SendMessageFromSocket(int port)
        {
            byte[] bytes = new byte[1024];

            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(endPoint);

            Console.WriteLine("Enter the request message:");
            string message = Console.ReadLine();

            Console.WriteLine("Socket is connecting with {0}.", sender.RemoteEndPoint.ToString());
            byte[] msg = Encoding.UTF8.GetBytes(message);
            int bytesSent = sender.Send(msg);
            int bytesReceived = sender.Receive(bytes);

            Console.WriteLine("\nServer's answer is: {0}.\n\n", Encoding.UTF8.GetString(bytes, 0, bytesReceived));

            if (message.IndexOf("<TheEnd>") == -1)
                SendMessageFromSocket(port);

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}
