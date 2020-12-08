using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataLibrary.Models
{
    [Table("RR")]
    public class RRmodel
    {
        [Key]
        public int RrID { get; set; }
        public int AaID { get; set; }
        public int UuID { get; set; }
        public int KsID { get; set; }
        public int KkID { get; set; }
        public string Drm { get; set; }
        public string Info { get; set; }
        public DateTime BasTrh { get; set; }
        public int Sure { get; set; }   // Dakika
        public DateTime BitTrh { get; set; }
        public DateTime? EXD { get; set; }
        public string Nots { get; set; }

        [NotMapped]
        public string UuAd { get; set; }

        [NotMapped]
        public string KsAd { get; set; }

        public string UuIDs { get; set; }

        [NotMapped]
        public string UuADs { get; set; }

        [NotMapped]
        public DateTime? _BasTrhD => BasTrh.Date;
        [NotMapped]
        public string _BasTrhT => BasTrh.ToString("HH:mm");
        [NotMapped]
        public string _BitTrhT => BitTrh.ToString("HH:mm");

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
    }
}
