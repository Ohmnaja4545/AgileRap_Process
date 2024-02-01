using AgileRap_Process.Data;
using System.ComponentModel.DataAnnotations;

namespace AgileRap_Process.Models
{
    public class WorkLogMetadata
    {
    }

    [MetadataType(typeof(WorkLogMetadata))]
    public partial class WorkLog
    {
        public void InsertData(AgileContext dbContext, Work work, int updateBy)
        {
            this.WorkID = work.ID;
            this.Project = work.Project;
            this.Name = work.Name;
            this.DueDate = work.DueDate;
            this.StatusID = work.StatusID;
            this.Remark = work.Remark;
            this.CreateBy = GlobalVariable.GetUserID();
            this.UpdateBy = updateBy;
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.IsDelete = false;
            dbContext.workLog.Add(this);
        }
    }
}
