using AgileRap_Process.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgileRap_Process.Models
{
    public class WorkMetadata
    {     
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    [MetadataType(typeof(WorkMetadata))]
    public partial class Work
    {
        [NotMapped]
        public string? ProviderValue { get; set; }
        [NotMapped]
        public bool? areAllSelected { get; set; }


        public void InsertData(AgileContext dbContext)
        {
            this.CreateBy = GlobalVariable.GetUserID();
            this.CreateDate = DateTime.Now;
            this.UpdateBy = GlobalVariable.GetUserID();
            this.UpdateDate = DateTime.Now;
            this.IsDelete = false;
            dbContext.works.Add(this);
        }

    }
}
