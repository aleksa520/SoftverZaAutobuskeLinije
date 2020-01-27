using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Domen;
using System.Net;
using System.Threading;

namespace Server
{
    public class Server
    {
        Socket osluskujuciSoket;
        public bool pokreniServer()
        {
            try
            {
                osluskujuciSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                osluskujuciSoket.Bind(new IPEndPoint(IPAddress.Parse("127.1.0.0"), 9000));
 
                ThreadStart ts = osluskuj;
                new Thread(ts).Start();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        //public bool zaustaviServer()
        //{
        //    try
        //    {
        //        osluskujuciSoket.Close();
        //        return true;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}

        void osluskuj()
        {
            try
            {
                while (true)
                {
                    osluskujuciSoket.Listen(5);
                    Socket klijent = osluskujuciSoket.Accept();
                    NetworkStream stream = new NetworkStream(klijent);
                    new NitKlijenta(stream);
                }
              
            }
            catch (Exception)
            {

                
            }

        }


    }
}
