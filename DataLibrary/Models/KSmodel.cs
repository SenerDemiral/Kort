using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLibrary.Models
{
    [Table("KS")]
    public class KSmodel
    {
        [Key]
        public int KsID { get; set; }
        public int AaID { get; set; }
        public int? Aktif { get; set; }
        [Required]
        [StringLength(20)]
        public string Ad { get; set; }
        public int? UyeRzrOK { get; set; }
    }
}
