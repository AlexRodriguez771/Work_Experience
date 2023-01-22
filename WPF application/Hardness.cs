using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_application
{
    public class Hardness
    {
        public Hardness(int _ID, string _KEY, string _Value)
        {
            ID = _ID;
            KEY = _KEY;
            Value = _Value;

        }

        public int ID { get; set; }
        public string KEY { get; set; }    
        public string Value { get; set; }   
    }
}
