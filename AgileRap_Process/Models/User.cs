namespace AgileRap_Process.Models
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class User
    {
        public int ID { get; set; }
        [Required]
        public string? Name { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; } 

        [DataType(DataType.EmailAddress)]
        [Required]
        public string? Email { get; set; }    
        public string? LineID { get; set; }
        public string? Role { get; set; }
        public int? CreateBy { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
        public virtual ICollection<Provider> Providers { get; set; }

        public virtual ICollection<ProviderLog> providerLogs { get; set; }

    }
}
