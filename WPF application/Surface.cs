using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_application
{
    public class Surface
    {
        public Surface(int _sID, string _sKEY, string _sValue)
        {
            sID = _sID;
            sKEY = _sKEY;
            sValue = _sValue;
        }
        public int sID { get; set; }
        public string sKEY { get; set; }

        public string sValue { get; set; }
    }
}
