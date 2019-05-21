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
    class ClientUsingProxy
    {
        private Socket _clientSocket;

        public ClientUsingProxy(Socket client)
        {
            this._clientSocket = client;
        }

        public void StartClientProcess()
        {
            Thread inProcess = new Thread(ClientProcessing);
            inProcess.Priority = ThreadPriority.AboveNormal;
            inProcess.Start();
        }

        private void ClientProcessing()
        {
            string reqLoad = "";
            string reqTemp = "";

            List<string> reqList = new List<string>(); // List cac request
            byte[] resBuffer = new byte[1]; // Tra loi
            byte[] reqBuffer = new byte[1]; // Yeu cau 
            string endLine = "\r\n";
            bool isRequested = true;
            reqList.Clear();
            // Proxy Server nhận các yêu cầu từ Client
            try
            {
                while (isRequested)
                {
                    this._clientSocket.Receive(reqBuffer);
                    string strByte = ASCIIEncoding.ASCII.GetString(reqBuffer);
                    reqLoad += strByte;
                    reqTemp += strByte;

                    if (reqTemp.EndsWith(endLine))
                    {
                        reqList.Add(reqTemp.Trim()); //Xóa tất cả kí tự khoảng trắng đầu và cuối chuỗi
                        reqTemp = "";
                    }
                    //Header kết thúc khi xuất hiện 2 lần ki tu \r\n
                    if (reqLoad.EndsWith(endLine + endLine))
                    {
                        isRequested = false;
                    }
                }

                Console.WriteLine(reqLoad);

                //Proxy Server gửi yêu cầu đến Server
                string getDomName = reqList[0].Split(' ')[1].Replace("http://", "").Split('/')[0];
                string reqInfor = reqList[0].Replace("http://", "").Replace(getDomName, "");
                reqList[0] = reqInfor;

                reqLoad = "";
                foreach (string line in reqList)
                {  //Lưu các gói tin 
                    reqLoad += line;
                    reqLoad += endLine;
                }
                /// Kiểm tra trong Blacklist 
                int numDomain;
                string[] Domain = new string[50]; // Danh sách Domain Name bị cấm
                CheckBlackList CheckBlackList;
                numDomain = CheckBlackList.Checking(Domain);
                String RepHTTP = "HTTP/1.1 403 Forbidden\r\n\r\n" + "<h1>403 Forbidden HTTP Response: You don't have permision to access</h1>";
                bool isAllowed = true;
                for (int i = 0; i < numDomain; i++)
                {
                    if (getDomName == Domain[i])
                    {
                        isAllowed = false;
                        this._clientSocket.Send(ASCIIEncoding.ASCII.GetBytes(RepHTTP));
                        break;
                    }
                }
                if (isAllowed == true)
                { //Tiếp tục kết nối
                    Socket Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    Server.Connect(getDomName, 80);

                    Server.Send(ASCIIEncoding.ASCII.GetBytes(reqLoad)); //Proxy Server gửi yêu cầu đến máy chủ
                    while (Server.Receive(resBuffer) != 0)
                        this._clientSocket.Send(resBuffer); //Proxy Server nhận trả lời từ máy chủ và trả về cho client

                    Server.Disconnect(false);
                    Server.Dispose();
                    this._clientSocket.Disconnect(false);
                    this._clientSocket.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occured: " + e.Message);
            }
        }
    }
}

