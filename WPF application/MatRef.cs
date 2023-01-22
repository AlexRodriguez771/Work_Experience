using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_application
{
    public class MatRef //The class is public becase it will be used for the last method I make
    {
       public MatRef(int _ID, string _US, string _DIN, string _UK, string _JIS, string _Classification)
        {
            ID = _ID;
            US = _US;
            DIN = _DIN;
            UK = _UK;
            JIS = _JIS;
            Classification = _Classification;

        }
        public int ID { get; set; }
        public string US { get; set; }
        public string DIN { get; set; }
        public string UK { get; set; }          //This class is used for the last method I made. Each variable set here is for a different standard of Metals and the classification is for the classification of the material  
        public string JIS { get; set; }
        public string Classification { get; set; }

    }

}



