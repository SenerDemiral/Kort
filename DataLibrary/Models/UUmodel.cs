using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataLibrary.Models
{
    [Table("UU")]
    public class UUmodel
    {
        [Key]
        public int UuID { get; set; }

        [NotMapped]
        public string IDS { get; set; }

        public int AaID { get; set; }
        public string Skl { get; set; } // Admin,Yetkili,Hoca,Grup,Ogrenci,Uye,Misafir

        [Required]
        [StringLength(40)]
        public string Ad { get; set; }
        
        public string Sex { get; set; }
        public DateTime? DgmTrh { get; set; }

        [StringLength(20)]
        public string Pwd { get; set; }
        
        [StringLength(20)]
        public string Tel1 { get; set; }
        
        [StringLength(20)]
        public string Tel2 { get; set; }

        [StringLength(40)]
        public string Mail1 { get; set; }

        [StringLength(40)]
        public string Mail2 { get; set; }
        public int MailOK { get; set; } = -1; // eMail alirim
        public int SmsOK { get; set; } = -1;  // SMS alirim

        public DateTime? BitTrh { get; set; }
        public DateTime? EXD { get; set; }
        public string Info { get; set; }
        
        public string UuIDs { get; set; }
        private IEnumerable<string> _SelectedUuIDs;
        [NotMapped]
        public IEnumerable<string> SelectedUuIDs
        {
            get
            {
                _SelectedUuIDs = UuIDs?.Split(',').ToList();
                return _SelectedUuIDs;
            }
            set
            {
                if (value == null)
                    UuIDs = null;
                else
                    UuIDs = string.Join(',', value);
                _SelectedUuIDs = value;
            }
        }

        public string TGs { get; set; }
        private IEnumerable<string> _SelectedTGs;
        [NotMapped]
        public IEnumerable<string> SelectedTGs
        {
            get
            {
                _SelectedTGs = TGs?.Split(',').ToList();
                return _SelectedTGs;
            }
            set
            {
                if (value == null)
                    TGs = null;
                else
                    TGs = string.Join(',', value);
                _SelectedTGs = value;
            }
        }

    }
}
