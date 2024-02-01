namespace AgileRap_Process.Models
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Work
    {
        public int ID { get; set; }
        public int? HeaderID { get; set; }
        //[Required]
        public string? Project {  get; set; }
        //[Required]
        public string? Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DueDate {  get; set; }     
        public int? StatusID { get; set; }
        public string? Remark { get; set; }
        public int? CreateBy {  get; set; }
        [DataType(DataType.Date)]

        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
        public bool? IsDelete { get; set; }

        public virtual ICollection<Provider> Provider { get; set; }
        public virtual ICollection<WorkLog> WorkLog { get; set; }

        public virtual Status? Status { get; set; }

    }
}
    