using System;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using NLog;

namespace EchoServerWindowsService
{
    public partial class EchoServer : ServiceBase
    {
        public EchoServer()
        {
            InitializeComponent();

            logger.Info("Start EchoServer constructor.");
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("OnStart method is starting.");
            var task = new Task(StartEchoServer);
            task.Start();
        }

        protected override void OnStop()
        {
            logger.Info("OnStop method is starting.");
        }

        private void StartEchoServer()
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                while (true)
                {
                    logger.Info($"Wait a connection from the port {ipEndPoint}.");

                    Socket handler = sListener.Accept();

                    string data = null;
                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    string reply = $"\nThank you for the request.\nYour requested string is: '{data}'.\nIt has {data.Length.ToString()} characters.";
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        logger.Info("The server closed the connection to the client.");
                        break;
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

                OnStop();
            }
            catch (Exception exc)
            {
                logger.Error(exc.Message);
                OnStop();
            }
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
