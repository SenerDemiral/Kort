using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLibrary.Models
{
    [Table("AA")]
    public class AAmodel
    {
        [Key]
        public int AaID { get; set; }

        [StringLength(40)]
        public string Ad { get; set; }
        
        [StringLength(20)]
        public string Tel { get; set; }
        
        [StringLength(40)]
        public string Mail { get; set; }
        
        public string Info { get; set; }

        public int RzrMaxGun { get; set; }
    }
}
