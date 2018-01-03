namespace CommercialWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LoaiThanhVien_Quyen
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaLoaiTV { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string MaQuyen { get; set; }

        [StringLength(50)]
        public string GhiChu { get; set; }

        public virtual LoaiThanhVien LoaiThanhVien { get; set; }

        public virtual Quyen Quyen { get; set; }
    }
}
