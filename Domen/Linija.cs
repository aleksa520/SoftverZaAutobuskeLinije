using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
    [Serializable]
    public class Linija
    {
        [Browsable(false)]
        public int LinijaID { get; set; }
        public string NazivLinije { get; set; }
        public Stanica PocetnaStanica { get; set; }
        public Stanica KrajnjaStanica { get; set; }
        public List<Stanica> Medjustanice { get; set; }
        public string Međustanice { get
            {
                string vrati = "";

                int ukupno = Medjustanice.Count();
                int trenutno = 0;
                foreach(Stanica s in Medjustanice)
                {
                    if(trenutno == ukupno - 1)
                    {
                        vrati += s.NazivStanice;
                        break;
                    }
                    vrati += s.NazivStanice + ", ";
                    trenutno++;
                }
                return vrati;
            } 
        }

        public override string ToString()
        {
            return NazivLinije;
        }
    }
}
