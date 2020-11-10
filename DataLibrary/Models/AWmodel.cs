using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLibrary.Models
{
    [Table("AW")]
    public class AWmodel
    {
        [Key]
        public int WD { get; set; }
        public int AaID { get; set; }
        public int Bas { get; set; }    // Dakika cinsinde 8 * 60
        public int Bit { get; set; }

        [StringLength(20)]
        public string Ad { get; set; }
    }
}
