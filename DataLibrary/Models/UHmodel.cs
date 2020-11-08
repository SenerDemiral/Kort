using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLibrary.Models
{
    [Table("UH")]
    public class UHmodel
    {
        [Key]
        public int UhID { get; set; }
        public int AaID { get; set; }
        public int UuID { get; set; }
        public DateTime? BasTrh { get; set; }
        public DateTime? BitTrh { get; set; }
        public string Info { get; set; }
    }
}
