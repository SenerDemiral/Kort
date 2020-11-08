using System;

namespace DataLibrary.Models
{
    public class RRdrmModel
    {
        public DateTime Trh { get; set; }
        public string _TrhT => Trh.ToString("HH:mm");
        public int NDK { get; set; }    // NumberofDoluKort
        public int NBK { get; set; }    // NumberofBosKort
        public string Info { get; set; }
    }
}
