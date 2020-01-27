using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
    public enum Operacije { Kraj = 1,
        VratiStanice = 2,
        SacuvajLiniju = 3
    }
    [Serializable]
    public class TransferKlasa
    {
        public Operacije operacija { get; set; }

        public Object TransferObjekat { get; set; }

        public Object Rezultat { get; set; }
    }
}
