using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
    [Serializable]
    public class Stanica
    {
        [Browsable (false)]
        public int StanicaId { get; set; }
        public string NazivStanice { get; set; }
        public override string ToString()
        {
            return NazivStanice;
        }
    }
}
