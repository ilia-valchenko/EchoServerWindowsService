using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EchoServerWindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var ServicesToRun = new ServiceBase[] { new EchoServer() };
            ServiceBase.Run(ServicesToRun);
            
            ServicesToRun[0].Stop();
        }
    }
}
