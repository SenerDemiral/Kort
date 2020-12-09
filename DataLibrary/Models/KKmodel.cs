using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLibrary.Models
{
    [Table("KK")]
    public class KKmodel
    {
        [Key]
        public int KkID { get; set; }
        public int AaID { get; set; }
        public int KsID { get; set; }
        public int? UyeOK { get; set; }

        [StringLength(20)]
        public string Ad { get; set; }
        [NotMapped]
        public string Info { get; set; }
    }
}
