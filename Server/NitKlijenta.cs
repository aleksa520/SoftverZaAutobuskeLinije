using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class NitKlijenta
    {
        private NetworkStream stream;
        BinaryFormatter formatter;

        public NitKlijenta(NetworkStream stream)
        {
            this.stream = stream;
            formatter = new BinaryFormatter();

            ThreadStart ts = obradi;
            new Thread(ts).Start();
        }
        void obradi()
        {
            try
            {
                int operacija = 0;
                while (operacija != (int)Operacije.Kraj)
                {
                    TransferKlasa transfer = formatter.Deserialize(stream) as TransferKlasa;
                    switch (transfer.operacija)
                    {
                        case Operacije.Kraj:
                            operacija = 1;
                            break;
                        case Operacije.VratiStanice:
                            transfer.Rezultat = Broker.Instance.VratiStanice();
                            formatter.Serialize(stream, transfer);
                            break;
                        case Operacije.SacuvajLiniju:
                            transfer.Rezultat = Broker.Instance.SacuvajLiniju((Linija)transfer.TransferObjekat);
                            formatter.Serialize(stream, transfer);
                            break;
                        default:

                            break;
                    }
                }


            }
            catch (Exception)
            {

            }
        }
    }
}
