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
    class CheckBlackList
    {
        public static int Checking(string[] msg)
        {
            System.IO.FileStream f;// doc blacklist
            f = new System.IO.FileStream(@"blacklist.conf", FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader fr = new StreamReader(f);
            Console.WriteLine("Blacklist:");
            string str = fr.ReadLine();
            int i = 0;
            while (str != null)
            {
                Console.WriteLine(str);
                msg[i] = str;
                i++;
                str = fr.ReadLine();
            }
            fr.Close();
            return i + 1;
        }
    }
}
