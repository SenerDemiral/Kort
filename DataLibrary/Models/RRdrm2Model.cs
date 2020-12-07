using System;
using System.Collections.Generic;

namespace DataLibrary.Models
{
    public class RRdrm2HdrModel
		{
        public int KkID { get; set; }
        public string KkAd { get; set; }
        public int X { get; set; }
        public int DimY { get; set; }

    }

    public class RRdrm2Model
    {
        public int X { get; set; }
        public int Y { get; set; }
        public DateTime Zmn { get; set; }
        public int RrID { get; set; }
    }

    public class RRdrm2ViewModel
		{
        public DateTime Zmn { get; set; }
        public Dictionary<int,int> RrID { get; set; }
    }
}
