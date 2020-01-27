using Domen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Broker
    {
        private static Broker instance;
        private SqlConnection connection;
        private SqlCommand command;
        SqlTransaction transaction;

        public static Broker Instance
        {
            get
            {
                if (instance == null) instance = new Broker();
                return instance;
            }
        }
        private Broker()
        {
            connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProSoft-Jun2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        internal BindingList<Linija> PovuciSveLinije()
        {
            BindingList<Linija> linije = new BindingList<Linija>();
            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = "SELECT l.LinijaID, l.NazivLinije, ps.StanicaID, ps.NazivStanice, ks.StanicaID, ks.NazivStanice FROM Linija l JOIN Stanica ps on(l.PocetnaStanica = ps.StanicaID) JOIN Stanica ks on(l.KrajnjaStanica = ks.StanicaID)";
                SqlDataReader reader = command.ExecuteReader();
                Linija l = null;
                while (reader.Read())
                {
                    l = new Linija()
                    {
                        LinijaID = reader.GetInt32(0),
                        NazivLinije = reader.GetString(1),
                        PocetnaStanica = new Stanica()
                        {
                            StanicaId = reader.GetInt32(2),
                            NazivStanice = reader.GetString(3)
                        },
                        KrajnjaStanica = new Stanica()
                        {
                            StanicaId = reader.GetInt32(4),
                            NazivStanice = reader.GetString(5)
                        }                      
                    };
                    //l.Medjustanice = VratiStaniceZaLiniju(l);
                    linije.Add(l);
                }
                reader.Close();
                return linije;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public List<Stanica> VratiStaniceZaLiniju(Linija linija)
        {
            connection.Open();
            List<Stanica> medjustanice = new List<Stanica>();
            SqlCommand command2 = connection.CreateCommand();
            command2.CommandText = $"Select ls.StanicaID, s.NazivStanice FROM LinijaStanica ls JOIN Stanica s on(ls.StanicaID = s.StanicaID) WHERE ls.LinijaID = {linija.LinijaID}";
            SqlDataReader reader = command2.ExecuteReader();

            while (reader.Read())
            {
                Stanica s = new Stanica()
                {
                    StanicaId = reader.GetInt32(0),
                    NazivStanice = reader.GetString(1)
                };
                medjustanice.Add(s);
            }
            reader.Close();
            connection.Close();
            return medjustanice;
        } 

        internal List<Stanica> VratiStanice()
        {
            List<Stanica> stanice = new List<Stanica>();
            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM STANICA";
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Stanica s = new Stanica()
                    {
                        StanicaId = reader.GetInt32(0),
                        NazivStanice = reader.GetString(1)
                    };
                    stanice.Add(s);
                }
                reader.Close();
                return stanice;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        internal bool SacuvajLiniju(Linija linija)
        {
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                if (!proveri(linija)) return false;
                command = new SqlCommand("", connection, transaction);
                command.CommandText = $"INSERT INTO Linija (NazivLinije, PocetnaStanica, KrajnjaStanica) VALUES('{linija.NazivLinije}',{linija.PocetnaStanica.StanicaId},{linija.KrajnjaStanica.StanicaId})";
                command.ExecuteNonQuery();
                int idLinije = IDLinije();

                foreach (Stanica medjustan in linija.Medjustanice)
                {
                    SqlCommand command2 = new SqlCommand("", connection, transaction);
                    command2.CommandText = $"INSERT INTO LinijaStanica VALUES({idLinije},{medjustan.StanicaId})";
                    command2.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                connection.Close();
            }

        }

        private bool proveri(Linija linija)
        {
            List<Stanica> medjustaniceIzBaze = new List<Stanica>();
            command = new SqlCommand("", connection, transaction);
            command.CommandText = $"SELECT ls.StanicaID, s.NazivStanice FROM LinijaStanica ls JOIN Linija l on(ls.LinijaID = l.LinijaID) JOIN Stanica s on(s.StanicaID = ls.StanicaID) WHERE l.NazivLinije = '{linija.NazivLinije}'";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Stanica s = new Stanica()
                {
                    StanicaId = reader.GetInt32(0),
                    NazivStanice = reader.GetString(1)
                };
                medjustaniceIzBaze.Add(s);
            }
            reader.Close();
            if(medjustaniceIzBaze.Count == 0)
            {
                return true;
            }

            if (medjustaniceIzBaze.Count != linija.Medjustanice.Count) return true;

            foreach (Stanica medju in linija.Medjustanice)
            {
                bool ima = false;
                foreach (Stanica medjuBaza in medjustaniceIzBaze)
                {
                    if (medju.StanicaId == medjuBaza.StanicaId)
                    {
                        ima = true;
                    }
                }
                if (!ima) return true; 
            }
            return false;
        }

        private int IDLinije()
        {
            command = new SqlCommand("", connection, transaction);
            command.CommandText = "SELECT MAX(LinijaID) FROM Linija";
            int sifra = Convert.ToInt32(command.ExecuteScalar());
            return sifra;
        }
    }
}
