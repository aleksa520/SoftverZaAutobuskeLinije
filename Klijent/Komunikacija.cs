using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Klijent
{
    public class Komunikacija
    {
        TcpClient klijent;
        NetworkStream stream;
        BinaryFormatter formatter;

        public bool poveziSe()
        {
            try
            {
                klijent = new TcpClient("127.1.0.0", 9000);
                stream = klijent.GetStream();
                formatter = new BinaryFormatter();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        internal List<Stanica> VratiStanice()
        {
            List<Stanica> stanice = new List<Stanica>();

            TransferKlasa transfer = new TransferKlasa();
            transfer.operacija = Operacije.VratiStanice;
            formatter.Serialize(stream, transfer);

            transfer = (TransferKlasa)formatter.Deserialize(stream);
            stanice = (List<Stanica>)transfer.Rezultat;
            return stanice;
        }

        public void kraj()
        {
            TransferKlasa transfer = new TransferKlasa();
            transfer.operacija = Operacije.Kraj;
            formatter.Serialize(stream, transfer);
        }

        internal bool SacuvajLiniju(string nazivLinije, Stanica pocetna, Stanica krajnja, List<Stanica> medjustaniceParam)
        {
            Linija linija = new Linija()
            {
                NazivLinije = nazivLinije,
                PocetnaStanica = pocetna,
                KrajnjaStanica = krajnja,
                Medjustanice = medjustaniceParam
            };

            TransferKlasa transfer = new TransferKlasa();

            transfer.operacija = Operacije.SacuvajLiniju;
            transfer.TransferObjekat = linija;
            formatter.Serialize(stream, transfer);

            transfer = (TransferKlasa)formatter.Deserialize(stream);
            if ((bool)transfer.Rezultat)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
