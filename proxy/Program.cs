using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectServer Proxy_Server = new ConnectServer(8888);
            Proxy_Server.StartServer();
            while (true)
                Proxy_Server.AllowClientConnection();
        }
    }
}
