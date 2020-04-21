using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geneticos2.Classes
{
    class Limit
    {
        private int m;
        
        public double A { set; get; }
        public double B { set; get; }
        public int M { 
            set { m = value < 0 ? -value : value; } 
            get => m; 
        }
    }
}
