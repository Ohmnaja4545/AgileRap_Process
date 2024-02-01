namespace AgileRap_Process.Models
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections.Generic;
    public partial class Status
    {
        public int ID { get; set; }
        public string? StatusName { get; set; }
        public int? CreateBy { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }    
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }     
        public virtual ICollection<Work> Work { get; set; }
        public virtual ICollection<WorkLog> workLogs { get; set; }


    }
}
