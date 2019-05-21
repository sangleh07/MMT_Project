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
    class ConnectServer
    {  ///Server lắng nghe kết nối
        private TcpListener _listener;
        private int _listenPort;

        public ConnectServer(int port)
        {
            this._listenPort = port;
            this._listener = new TcpListener(IPAddress.Any, this._listenPort);
        }

        public void StartServer()
        {  //Server bắt đầu lắng nghe các yêu cầu
            this._listener.Start();
        }

        public void AllowClientConnection()
        {
            //Bắt đầu cho các Client kết nối với Proxy Server
            Socket newClient = this._listener.AcceptSocket();  //Gán Client cho biến Socket để làm việc với Proxy Server
            ClientUsingProxy client = new ClientUsingProxy(newClient);
            client.StartClientProcess();
        }

    }
}

