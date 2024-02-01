using AgileRap_Process.Data;
using System.ComponentModel.DataAnnotations;

namespace AgileRap_Process.Models
{
    public class ProviderLogMetadata
    {
    }
    [MetadataType(typeof(ProviderLogMetadata))]
    public partial class ProviderLog
    {
        public void InsertData(AgileContext dbContext, Provider newprovider, Work work)
        {
            this.UserID = newprovider.UserID;
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.CreateBy = GlobalVariable.GetUserID();
            this.UpdateBy = newprovider.UpdateBy;
            this.IsDelete = false;
            foreach (var workLogs in work.WorkLog)
            {
                this.WorkLogID = workLogs.ID;
            }
            dbContext.providersLog.Add(this);
        }
    }
}
