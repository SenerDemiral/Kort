using System;
using System.Collections.Generic;
using System.Text;

namespace DataLibrary.Models
{
    public class LoginResultModel
    {
        public string OK { get; set; }
        public int AaID { get; set; }
        public string UsrAd { get; set; } = "";
        public string UsrSkl { get; set; } = "";
        public string AaAd { get; set; } = "";
    }
}
