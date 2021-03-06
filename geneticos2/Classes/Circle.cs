﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geneticos2.Classes
{
    class Circle
    {
        private double r;

        public double X { get; set; }
        public double Y { set; get; }
        public double R {
            set { r = value < 0 ? -value : value; }
            get => r;
        }

        public override string ToString()
        {
            return X + " " + Y + " " + r;
        }
    }
}
