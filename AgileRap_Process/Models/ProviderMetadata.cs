using AgileRap_Process.Data;
using System.ComponentModel.DataAnnotations;

namespace AgileRap_Process.Models
{
    public class ProviderMetadata
    {



    }
    [MetadataType(typeof(ProviderMetadata))]
    public partial class Provider
    {
        public void InsertData(AgileContext dbContext, int? id, Work work)
        {
            this.UserID = id;
            this.WorkID = work.ID;
            this.Work = work;
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.CreateBy = GlobalVariable.GetUserID();
            this.UpdateBy = GlobalVariable.GetUserID();
            this.IsDelete = false;
            dbContext.providers.Add(this);
        }
        public void Insert2Data(AgileContext dbContext, int? id, Work work)
        {
            this.UserID = id;
            this.WorkID = work.ID;
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.CreateBy = GlobalVariable.GetUserID();
            this.UpdateBy = work.UpdateBy;
            this.IsDelete = false;
            dbContext.providers.Add(this);
        }
        public void AddData(int id, Work work)
        {
            this.UserID = id;
            this.WorkID = work.ID;
            this.Work = work;
            this.CreateBy = GlobalVariable.GetUserID();
            this.UpdateBy = work.UpdateBy;
            this.Work = work;
            this.IsDelete = false;
            work.Provider.Add(this);
        }
        public void AddOldData(Work work, ProviderLog worklogsame)
        {
            this.User = worklogsame.User;
            this.UserID = worklogsame.UserID;
            this.WorkID = worklogsame.ID;
            this.Work = work;
            this.CreateBy = worklogsame.CreateBy;
            this.UpdateBy = worklogsame.UpdateBy;
            this.IsDelete = false;
            work.Provider.Add(this);
        }



    }
}
