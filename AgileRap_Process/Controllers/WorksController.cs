using AgileRap_Process.Data;
using AgileRap_Process.Helpter;
using AgileRap_Process.Models;
using AgileRap_Process.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;



namespace AgileRap_Process.Controllers
{
    public class WorksController : Controller
    {
        private AgileContext db = new AgileContext();
        private readonly IEmailService emailService;

        public WorksController(IEmailService emailService)
        {
            this.emailService = emailService;
        }


        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
                ViewBag.isLogin = true;
            }
            else
            {
                return RedirectToAction("Login");
            }

            var status = db.statuses.ToList();
            var work = db.works.Include(q => q.Provider).ToList();
            var providers = db.providers.ToList();
            var user = db.users.ToList();
            //foreach (var provider in providers)
            //{
            //    provider.User = user.FirstOrDefault(u => u.ID == provider.UserID);
            //}
            //foreach (var w in work)
            //{
            //    foreach (var provider in w.Provider)
            //    {
            //        if (provider.ID == w.ID)
            //        {
            //            w.Provider = providers;
            //        }
            //    }
            //}
            ViewBag.status = status;
            ViewBag.work = work;
            ViewBag.provider = providers;
            ViewBag.user = user;
            return View(db.works.ToList());
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var myUser = db.users.Where(m => m.Email == user.Email && m.Password == user.Password).FirstOrDefault();
            if (myUser != null)
            {
                GlobalVariable.SetUserID(myUser.ID);
                HttpContext.Session.SetString("UserSession", myUser.Email);
                user.isLogin = true;
                ViewBag.isLogin = user.isLogin;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Login Failed...";
            }

            return View();
        }
        public ActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult Registor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registor(User user)
        {

            var filterEmail = db.users.Where(m => m.Email == user.Email).FirstOrDefault();
            if (user.Password != user.ConfirmPassword)
            {
                TempData["PasswordNotmatch"] = "Password and ConfirmPassword not match";
                return RedirectToAction("Registor");
            }
            if (filterEmail != null)
            {
                TempData["DuplicateEmail"] = "Register Duplicate Email";
                return RedirectToAction("Registor");
            }
            else
            {
                db.users.Add(user);
                db.SaveChanges();
                TempData["Success"] = "Register Successfully";
                return RedirectToAction("Login");
            }
        }
        public ActionResult Test()
        {
            var status = db.statuses.ToList();
            var work = db.works.Include(q => q.Provider).ToList();
            var providers = db.providers.ToList();
            var user = db.users.ToList();

            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
                var e = HttpContext.Session.GetString("UserSession").ToString();
                var u = db.users.FirstOrDefault(u => u.Email == e);
                ViewBag.CreateByID = u.ID;
                ViewBag.isLogin = true;

            }
            else
            {
                return RedirectToAction("Login");
            }

            //foreach (var provider in providers)
            //{
            //    provider.User = user.FirstOrDefault(u => u.ID == provider.UserID);
            //}
            //foreach (var w in work)
            //{
            //    foreach (var provider in w.Provider)
            //    {
            //        if (provider.ID == w.ID)
            //        {
            //            w.Provider = providers;

            //        }
            //    }
            //}

            ViewBag.status = status;
            ViewBag.work = work;
            ViewBag.provider = providers;
            ViewBag.user = user;

            return View();
        }

        public ActionResult Create()
        {
            List<SelectListItem> selectStatus = new List<SelectListItem>();

            foreach (var i in db.statuses.ToList())
            {
                SelectListItem item = new SelectListItem() { Text = i.StatusName, Value = i.ID.ToString() };
                selectStatus.Add(item);
            }
            ViewBag.selectStatus = selectStatus;

            var works = db.works.ToList();
            Work work = new Work();
            works.Add(work);

            var status = db.statuses.ToList();
            var provider = db.providers.ToList();
            var user = db.users.ToList();
            foreach (var i in provider)
            {
                i.User = user.FirstOrDefault();
            }
            ViewBag.status = status;
            ViewBag.work = works;
            ViewBag.provider = provider;
            ViewBag.user = user;
            return View(works);
        }

        [HttpPost]
        public ActionResult Create(Work work)
        {
            SaveWorkDB(work);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private void SaveWorkDB(Work work)
        {
            var user = db.users.ToList();
            var e = HttpContext.Session.GetString("UserSession").ToString();
            var u = db.users.FirstOrDefault(u => u.Email == e);
            if (work.ID == 0)
            {
                ////-----ไม่ใช้แล้ว-----
                ////var status = work.Status; 
                ////work.DueDate = work.DueDate.Value.AddYears(543);
                ////work.Status = db.statuses.Find(work.Status.ID); // Assuming 2 is the correct StatusID
                ////work.StatusID = work.Status.ID;
                ////-----ไม่ใช้แล้ว-----

                //work.CreateDate = DateTime.Now;
                //work.UpdateDate = DateTime.Now;
                //work.CreateBy = u.ID;
                //work.UpdateBy = u.ID;
                //work.IsDelete = false;
                //db.works.Add(work);

                work.InsertData(db);

                db.SaveChanges();
                SaveWorkLogDB(work);


                if (work.areAllSelected == true)
                {
                    foreach (var i in user)
                    {
                        Provider newprovider = new Provider();
                        //newprovider.User = user.FirstOrDefault(u => u.ID == newprovider.UserID);
                        //newprovider.UserID = i.ID;
                        //newprovider.WorkID = work.ID;
                        //newprovider.Work = work;
                        //newprovider.CreateDate = DateTime.Now;
                        //newprovider.UpdateDate = DateTime.Now;
                        //newprovider.CreateBy = work.CreateBy;
                        //newprovider.UpdateBy = work.UpdateBy;
                        //newprovider.IsDelete = false;
                        //db.providers.Add(newprovider);

                        newprovider.InsertData(db, i.ID, work);
                        //Create provider


                        ProviderLog providerLog = new ProviderLog();
                        //providerLog.UserID = newprovider.UserID;
                        //providerLog.CreateDate = DateTime.Now;
                        //providerLog.UpdateDate = DateTime.Now;
                        //providerLog.CreateBy = newprovider.CreateBy;
                        //providerLog.UpdateBy = newprovider.UpdateBy;
                        //providerLog.IsDelete = false;

                        //foreach (var workLogs in work.WorkLog)
                        //{
                        //    providerLog.WorkLogID = workLogs.ID;
                        //}
                        //db.providersLog.Add(providerLog);

                        providerLog.InsertData(db, newprovider, work); //Create providerLog

                    }
                }
                else
                {
                    int[] listprovider = Array.ConvertAll(work.ProviderValue.Split(','), int.Parse);
                    foreach (int i in listprovider)
                    {
                        Provider newprovider = new Provider();
                        //newprovider.User = user.FirstOrDefault(u => u.ID == newprovider.UserID);

                        //newprovider.UserID = i;
                        //newprovider.WorkID = work.ID;
                        //newprovider.Work = work;
                        //newprovider.CreateDate = DateTime.Now;
                        //newprovider.UpdateDate = DateTime.Now;
                        //newprovider.CreateBy = work.CreateBy;
                        //newprovider.UpdateBy = work.UpdateBy;
                        //newprovider.IsDelete = false;
                        //db.providers.Add(newprovider);

                        newprovider.InsertData(db, i, work);

                        ProviderLog providerLog = new ProviderLog();
                        //providerLog.UserID = newprovider.UserID;
                        //providerLog.CreateDate = DateTime.Now;
                        //providerLog.UpdateDate = DateTime.Now;
                        //providerLog.CreateBy = newprovider.CreateBy;
                        //providerLog.UpdateBy = newprovider.UpdateBy;
                        //providerLog.IsDelete = false;

                        //foreach (var workLogs in work.WorkLog)
                        //{
                        //    providerLog.WorkLogID = workLogs.ID;
                        //}

                        //db.providersLog.Add(providerLog);
                        providerLog.InsertData(db, newprovider, work);
                    }
                }
            }
            else
            {
                work.UpdateBy = u.ID;
                db.Entry(work).State = EntityState.Modified;

            }
            FindEmailforSend(work);
        }
        private async void FindEmailforSend(Work work)
        {
            List<string?> ListEmail = new List<string?>();
            var userCreateby = db.users.FirstOrDefault(u => u.ID == work.CreateBy);

            foreach (var p in work.Provider)
            {
                var userProvider = db.users.FirstOrDefault(a => a.ID == p.UserID);
                ListEmail.Add(userProvider.Email); //Add email ของ provider
            }
            ListEmail.Add(userCreateby.Email); //Add email ของ creater
            await SendMail(ListEmail);

        }
        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail(List<string?> ListEmailforsend)
        {
            //List<string?> ListmailProvider = new List<string?>();
            //ListmailProvider.Add("passakorn2.fivem5@gmail.com");
            //ListmailProvider.Add("passakorn.fivem5@gmail.com");

            try
            {
                string body;
                string subject = "Welcome to Agiles";
                foreach (var l in ListEmailforsend)
                {
                    if (l == ListEmailforsend.Last())
                    {
                        // ข้อความของคนสร้างโปรเจค
                        body = "You have successfully created your project.";
                    }
                    else
                    {
                        // ข้อความคนที่ได้รับงาน
                        body = "You have received a new job.";
                    }
                    Mailrequest mailrequest = new Mailrequest();
                    mailrequest.ToEmail = l;
                    mailrequest.Subject = subject;
                    mailrequest.Body = body;
                    await emailService.SendEmailAsync(mailrequest);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private bool AreWorkAndWorkLogEqual(Work work, WorkLog workLog)
        {
            return work.Project == workLog.Project
                && work.Name == workLog.Name
                && work.DueDate == workLog.DueDate
                && work.StatusID == workLog.StatusID
                && work.Remark == workLog.Remark;

        }
        private void SaveWorkLogDB(Work work)
        {
            var e = HttpContext.Session.GetString("UserSession").ToString();
            var u = db.users.FirstOrDefault(u => u.Email == e);
            WorkLog workLog = new WorkLog();
            //workLog.WorkID = work.ID;
            //workLog.Project = work.Project;
            //workLog.Name = work.Name;
            //workLog.DueDate = work.DueDate;
            //workLog.StatusID = work.StatusID;
            //workLog.Remark = work.Remark;
            //workLog.CreateBy = work.CreateBy;
            //workLog.UpdateBy = u.ID;
            //workLog.CreateDate = DateTime.Now;
            //workLog.UpdateDate = DateTime.Now;
            //workLog.IsDelete = false;

            var checkdataWorkLog = db.workLog.FirstOrDefault(m => m.WorkID == work.ID);
            //var checkdataWorkLog2 = db.workLog.LastOrDefault();

            if (checkdataWorkLog != null) // create คัร้งที่ 2
            {
                if (work.ProviderValue != null)
                {
                    var user = db.users.ToList();
                    int[] listprovider = Array.ConvertAll(work.ProviderValue.Split(','), int.Parse);
                    foreach (int i in listprovider)
                    {
                        Provider newprovider = new Provider();
                        //newprovider.User = user.FirstOrDefault(u => u.ID == newprovider.UserID);
                        //newprovider.UserID = i;
                        //newprovider.WorkID = work.ID;
                        //newprovider.Work = work;
                        //newprovider.CreateBy = work.CreateBy;
                        //newprovider.UpdateBy = work.UpdateBy;
                        //newprovider.Work = work;
                        //newprovider.IsDelete = false;
                        //work.Provider.Add(newprovider);
                        newprovider.AddData(i, work);
                    }
                }
                if (work.ProviderValue == null && work.areAllSelected == null) // กรณี user กด แก้ไขแล้วกด submit คือข้อมูล ProviderValue == null
                {
                    var user = db.users.ToList();
                    var workLogs = work.WorkLog.ToList();
                    var lastWorkLog = work.WorkLog.LastOrDefault();

                    if (lastWorkLog != null)
                    {
                        if (lastWorkLog.providerLogs == null)
                        {
                            var skipCount = 1;
                            var LastOldworkLogskip1 = workLogs.SkipLast(1).LastOrDefault();

                            while (LastOldworkLogskip1.providerLogs == null)
                            {
                                LastOldworkLogskip1 = workLogs.SkipLast(skipCount).LastOrDefault();
                                skipCount++;
                            }

                            lastWorkLog.providerLogs = LastOldworkLogskip1.providerLogs;

                            foreach (var worklogsame in lastWorkLog.providerLogs)
                            {
                                Provider newprovider = new Provider();
                                //newprovider.User = worklogsame.User;
                                //newprovider.UserID = worklogsame.UserID;
                                //newprovider.WorkID = worklogsame.ID;                               
                                //newprovider.Work = work;
                                //newprovider.CreateBy = worklogsame.CreateBy;
                                //newprovider.UpdateBy = worklogsame.UpdateBy;
                                //newprovider.IsDelete = false;
                                //work.Provider.Add(newprovider);

                                newprovider.AddOldData(work, worklogsame);


                            }
                        }
                        else
                        {
                            //foreach (var worklogsame in lastWorkLog.providerLogs)
                            //{
                            //    Provider newprovider = new Provider();
                            //    //newprovider.User = worklogsame.User;
                            //    //newprovider.UserID = worklogsame.UserID;
                            //    //newprovider.WorkID = worklogsame.ID;
                            //    //newprovider.Work = work;
                            //    //newprovider.CreateBy = worklogsame.CreateBy;
                            //    //newprovider.UpdateBy = worklogsame.UpdateBy;
                            //    //newprovider.IsDelete = false;
                            //    //work.Provider.Add(newprovider);
                            //    newprovider.AddOldData(work, worklogsame);
                            //}
                        }
                    }
                }
                if (work.ProviderValue == null && work.areAllSelected == true)
                {
                    var user = db.users.ToList();
                    foreach (var i in user)
                    {
                        Provider newprovider = new Provider();
                        //newprovider.User = user.FirstOrDefault(u => u.ID == newprovider.UserID);
                        //newprovider.UserID = i.ID;
                        //newprovider.WorkID = work.ID;
                        //newprovider.Work = work;
                        //newprovider.CreateBy = work.CreateBy;
                        //newprovider.UpdateBy = work.UpdateBy;
                        //work.Provider.Add(newprovider);

                        newprovider.AddData(i.ID, work);
                    }
                }

                var checkdataWorkLog2 = db.workLog.Where(m => m.WorkID == work.ID)
                .OrderBy(m => m.No)
                .LastOrDefault();

                if (!AreWorkAndWorkLogEqual(work, checkdataWorkLog2))
                {
                    // Data has changed, update workLog
                    workLog.No = checkdataWorkLog2.No + 1;
                    work.WorkLog = new List<WorkLog> { workLog };

                    workLog.InsertData(db, work, u.ID);
                    //db.workLog.Add(workLog);
                    db.SaveChanges();
                }
                else
                {
                    var LastOldworkLog = work.WorkLog.LastOrDefault();
                    TempData["LastOldworkLogproviderLogs"] = LastOldworkLog.providerLogs.Count();

                    var workLogs = work.WorkLog.ToList();
                    List<int?> List1 = new List<int?>();
                    List<int?> List2 = new List<int?>();

                    foreach (var OldworkLog in work.WorkLog)
                    {
                        if (LastOldworkLog.providerLogs == null)
                        {
                            var skipCount = 1;
                            var LastOldworkLogskip1 = workLogs.SkipLast(1).LastOrDefault();
                            while (LastOldworkLogskip1.providerLogs == null)
                            {
                                LastOldworkLogskip1 = workLogs.SkipLast(skipCount).LastOrDefault();
                                skipCount++;
                            }
                            LastOldworkLog.providerLogs = LastOldworkLogskip1.providerLogs;
                        }
                        if (List1.Count == 0)
                        {
                            foreach (var OldproviderLog in LastOldworkLog.providerLogs) //เก่าล่าสุด
                            {
                                List1.Add(OldproviderLog.UserID);
                            }
                        }

                        if (work.Provider.Count == 0)
                        {
                            foreach (var worklogsame in LastOldworkLog.providerLogs)
                            {
                                Provider newprovider = new Provider();
                                //newprovider.User = worklogsame.User;
                                //newprovider.UserID = worklogsame.UserID;
                                //newprovider.WorkID = worklogsame.ID;
                                //newprovider.Work = work;
                                //newprovider.CreateBy = worklogsame.CreateBy;
                                //newprovider.UpdateBy = worklogsame.UpdateBy;
                                //newprovider.IsDelete = false;
                                //work.Provider.Add(newprovider);
                                newprovider.AddOldData(work, worklogsame);
                            }
                        }
                        if (List2.Count == 0)
                        {
                            foreach (var NewProvider in work.Provider) //ใหม่
                            {
                                List2.Add(NewProvider.UserID);

                            }
                        }


                        if (LastOldworkLog.providerLogs.Count == work.Provider.Count) // ifจำนวน เก่า == ใหม่
                        {
                            for (int i = 0; i < LastOldworkLog.providerLogs.Count - 1; i++)

                                if (List1[i] != List2[i]) //มีการแก้ไข provider
                                {
                                    TempData["checkeditPro"] = "yes";
                                    workLog.No = checkdataWorkLog2.No + 1;
                                    work.WorkLog = new List<WorkLog> { workLog };
                                    //db.workLog.Add(workLog);
                                    workLog.InsertData(db, work, u.ID);
                                    db.SaveChanges();
                                }

                        }
                        else // ไม่เท่ากับ
                        {
                            TempData["checkeditPro"] = "yes";
                            workLog.No = checkdataWorkLog2.No + 1;
                            work.WorkLog = new List<WorkLog> { workLog };
                            //db.workLog.Add(workLog);
                            workLog.InsertData(db, work, u.ID);
                            db.SaveChanges();
                            break;
                        }
                    }
                }
            }
            else //ครั้งแรก
            {
                workLog.No = 1;
                work.WorkLog = new List<WorkLog> { workLog };
                workLog.InsertData(db, work, u.ID);
                db.SaveChanges();

            }
            //SaveProviderLogDB(work);
        }

        //private void SaveProviderLogDB(Work work)
        //{
        //    var user = db.users.ToList();

        //    //SplitProviderValue(work);
        //    foreach (var provider in work.Provider)
        //    {
        //        ProviderLog providerLog = new ProviderLog();
        //        providerLog.UserID = provider.UserID;
        //        providerLog.CreateDate = DateTime.Now;
        //        providerLog.UpdateDate = DateTime.Now;
        //        providerLog.IsDelete = false;

        //        foreach (var workLogs in work.WorkLog)
        //        {
        //            providerLog.WorkLogID = workLogs.ID;
        //        }

        //        db.providersLog.Add(providerLog);
        //    }
        //    //SplitProviderValue(work);
        //}

        public ActionResult Edit(int? id)
        {
            var status = db.statuses.ToList();
            var work2 = db.works.ToList();
            var providers = db.providers.ToList();
            var worklog = db.workLog.ToList();
            var providerlog = db.providersLog.ToList();
            var user = db.users.ToList();

            Work work = db.works.Find(id);

            var selectedWork = work2.FirstOrDefault(w => w.ID == id);
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
                var e = HttpContext.Session.GetString("UserSession").ToString();
                var u = db.users.FirstOrDefault(u => u.Email == e);
                ViewBag.CreateByID = u.ID;
                ViewBag.UpdateByID = u.ID;
                ViewBag.isLogin = true;

            }

            else
            {
                return RedirectToAction("Login");
            }

            ViewBag.status = status;
            ViewBag.selectedWork = selectedWork;
            ViewBag.work2 = work2;
            ViewBag.provider = providers;
            ViewBag.user = user;

            return View(work);
        }

        [HttpPost]
        public ActionResult Edit(Work work)
        {
            work.Provider.Clear();
            var user = db.users.ToList();
            var providerlog = db.providersLog.ToList();
            var worklog = db.workLog.ToList();
            SaveWorkLogDB(work);
            if (work.ProviderValue != null && work.Provider.Count == 0)
            {
                int[] listprovider = Array.ConvertAll(work.ProviderValue.Split(','), int.Parse);

                var existingProviders = db.providers.Where(p => p.WorkID == work.ID);
                db.providers.RemoveRange(existingProviders);
                //db.SaveChanges();
                foreach (int i in listprovider)
                {
                    Provider newprovider = new Provider();
                    //newprovider.User = user.FirstOrDefault(u => u.ID == newprovider.UserID);
                    //newprovider.UserID = i;
                    //newprovider.WorkID = work.ID;
                    //newprovider.IsDelete = false;
                    //db.providers.Add(newprovider);
                    newprovider.InsertData(db, i, work);

                    ProviderLog providerLog = new ProviderLog();
                    //providerLog.UserID = newprovider.UserID;
                    //providerLog.CreateDate = DateTime.Now;
                    //providerLog.UpdateDate = DateTime.Now;
                    //providerLog.IsDelete = false;

                    //foreach (var workLogs in work.WorkLog)
                    //{
                    //    providerLog.WorkLogID = workLogs.ID;
                    //}

                    //db.providersLog.Add(providerLog);
                    providerLog.InsertData(db, newprovider, work);
                }

            }

            var lastproviderLogs = (int)TempData["LastOldworkLogproviderLogs"];
            TempData["usercount"] = db.users.ToList().Count();
            var userCount = (int)TempData["usercount"];
            if (lastproviderLogs < userCount) // ถ้า จำนวน Providerlog < User ทั้งหมด // เก่า < ใหม่
            {
                if (work.Provider != null && work.areAllSelected == true) //////######
                {
                    var existingProviders = db.providers.Where(p => p.WorkID == work.ID);
                    db.providers.RemoveRange(existingProviders);

                    foreach (var i in user)
                    {
                        Provider newprovider = new Provider();

                        //newprovider.User = user.FirstOrDefault(u => u.ID == newprovider.UserID);
                        //newprovider.UserID = i.ID;
                        //newprovider.WorkID = work.ID;
                        //newprovider.IsDelete = false;
                        //db.providers.Add(newprovider);
                        newprovider.Insert2Data(db, i.ID, work);
                        //newprovider.InsertData(db, i.ID, work);

                        //private bool AreWorkAndWorkLogEqual(Pro work, WorkLog workLog)
                        //{
                        //    return work.Project == workLog.Project
                        //        && work.Name == workLog.Name
                        //        && work.DueDate == workLog.DueDate
                        //        && work.StatusID == workLog.StatusID
                        //        && work.Remark == workLog.Remark;

                        //}
                        //foreach(var k in work.Provider)
                        //{
                        //    if()
                        //}
                        ProviderLog providerLog = new ProviderLog();
                        //providerLog.UserID = i.UserID;
                        //providerLog.CreateDate = DateTime.Now;
                        //providerLog.UpdateDate = DateTime.Now;
                        //providerLog.IsDelete = false;

                        //foreach (var workLogs in work.WorkLog)
                        //{
                        //    providerLog.WorkLogID = workLogs.ID;
                        //}

                        //db.providersLog.Add(providerLog);
                        providerLog.InsertData(db, newprovider, work);

                    }
                }
            }

            if (work.Provider != null && work.areAllSelected == false) /////####2222
            {
                var existingProviders = db.providers.Where(p => p.WorkID == work.ID);
                db.providers.RemoveRange(existingProviders);

                foreach (var i in work.Provider)
                {
                    Provider newprovider = new Provider();

                    //newprovider.User = user.FirstOrDefault(u => u.ID == newprovider.UserID);
                    //newprovider.UserID = i.UserID;
                    //newprovider.WorkID = work.ID;
                    //newprovider.IsDelete = false;
                    //db.providers.Add(newprovider);
                    newprovider.Insert2Data(db, i.UserID, work);



                    ProviderLog providerLog = new ProviderLog();
                    //providerLog.UserID = i.UserID;
                    //providerLog.CreateDate = DateTime.Now;
                    //providerLog.UpdateDate = DateTime.Now;
                    //providerLog.IsDelete = false;

                    //foreach (var workLogs in work.WorkLog)
                    //{
                    //    providerLog.WorkLogID = workLogs.ID;
                    //}

                    //db.providersLog.Add(providerLog);
                    providerLog.InsertData(db, newprovider, work);

                }
            }
            var cp = TempData["checkeditPro"];
            if (cp == "yes") //ถ้ามีการแก้ไข Provider
            {
                if (work.Provider != null && work.areAllSelected == null) //เพิ่มมาใหม่
                {
                    var existingProviders = db.providers.Where(p => p.WorkID == work.ID);
                    db.providers.RemoveRange(existingProviders);

                    foreach (var i in work.Provider)
                    {
                        Provider newprovider = new Provider();
                        newprovider.Insert2Data(db, i.UserID, work);

                        ProviderLog providerLog = new ProviderLog();
                        providerLog.InsertData(db, newprovider, work);

                    }
                }
            }

            if (work.areAllSelected == true && work.Provider == null || work.Provider == null)
            {
                var existingProviders = db.providers.Where(p => p.WorkID == work.ID);
                db.providers.RemoveRange(existingProviders);
                //db.SaveChanges();
                foreach (var i in user)
                {
                    Provider newprovider = new Provider();
                    //newprovider.User = user.FirstOrDefault(u => u.ID == newprovider.UserID);
                    //newprovider.UserID = i.ID;
                    //newprovider.WorkID = work.ID;
                    //newprovider.IsDelete = false;
                    //db.providers.Add(newprovider);
                    newprovider.InsertData(db, i.ID, work);


                    ProviderLog providerLog = new ProviderLog();
                    //providerLog.UserID = newprovider.UserID;
                    //providerLog.CreateDate = DateTime.Now;
                    //providerLog.UpdateDate = DateTime.Now;
                    //providerLog.IsDelete = false;

                    //foreach (var workLogs in work.WorkLog)
                    //{
                    //    providerLog.WorkLogID = workLogs.ID;
                    //}

                    //db.providersLog.Add(providerLog);
                    providerLog.InsertData(db, newprovider, work);

                }
            }
            db.SaveChanges();
            SaveWorkDB(work);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult History(int? id)
        {
            var status = db.statuses.ToList();
            var work2 = db.works.ToList();
            var providers = db.providers.ToList();
            var user = db.users.ToList();
            var worklog = db.workLog.ToList();
            var providerlog = db.providersLog.ToList();
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
                var e = HttpContext.Session.GetString("UserSession").ToString();
                var u = db.users.FirstOrDefault(u => u.Email == e);
                ViewBag.CreateByID = u.ID;
                ViewBag.isLogin = true;

            }
            else
            {
                return RedirectToAction("Login");
            }
            Work work = db.works.Find(id);

            var selectedWork = work2.FirstOrDefault(w => w.ID == id);

            ViewBag.status = status;
            ViewBag.selectedWork = selectedWork;
            ViewBag.work2 = work2;
            ViewBag.provider = providers;
            ViewBag.user = user;
            ViewBag.Name = work.Name;

            if (work.WorkLog != null)
            {
                List<WorkLog> workLogList = work.WorkLog.ToList();

                List<string> List0 = new List<string>();
                List<string> List1 = new List<string>();
                List<string> List2 = new List<string>();

                //for (int i = workLogList.Count -1; i >= 0 + 1; i--)
                for (int i = 0; i < workLogList.Count - 1; i++)
                {
                    bool isChange = false;

                    var x = i + 1;
                    //var log0 = (i > 0) ? workLogList[1 + (workLogList.Count - 1 - i)] : null;
                    //var log = workLogList[0 + (workLogList.Count - 1 - i)];
                    //var log2 = workLogList[1 + (workLogList.Count - 1 - i)];
                    var log0 = (i > 0) ? workLogList[i - 1] : null;
                    var log = workLogList[i];
                    var log2 = workLogList[i + 1];
                    if (log.providerLogs == null)
                    {
                        log.providerLogs = log0.providerLogs;
                    }
                    if (log2.providerLogs != null)
                    {
                        if (log.providerLogs.Count != log2.providerLogs.Count)
                        {
                            isChange = true;

                            List<string> ListUser = new List<string>();
                            List<string> ListUser2 = new List<string>();

                            foreach (var logProvider in log.providerLogs)
                            {
                                if (logProvider.UserID == logProvider.User.ID)
                                {
                                    ListUser.Add(logProvider.User.Name);
                                }
                            }
                            foreach (var logProvider2 in log2.providerLogs)
                            {
                                if (logProvider2.UserID == logProvider2.User.ID)
                                {
                                    ListUser2.Add(logProvider2.User.Name);
                                }
                            }
                            string result = string.Join(", ", ListUser);
                            string result2 = string.Join(", ", ListUser2);

                            List0.Insert(0, "");

                            List1.Insert(0, "Provider: " + result + "->");
                            List2.Insert(0, result2);

                        }
                        else
                        {
                            List<string> ListUser = new List<string>();
                            List<string> ListUser2 = new List<string>();

                            foreach (var logProvider in log.providerLogs)
                            {
                                if (logProvider.UserID != logProvider.User.ID)
                                {
                                    isChange = true;

                                    ListUser.Add(logProvider.User.Name);

                                }
                                else
                                {
                                    isChange = false;

                                }
                            }
                            foreach (var logProvider2 in log2.providerLogs)
                            {
                                if (logProvider2.UserID != logProvider2.User.ID)
                                {
                                    isChange = true;
                                    ListUser2.Add(logProvider2.User.Name);

                                }
                                else
                                {
                                    isChange = false;
                                }
                            }
                            //if (logProvider.UserID != logProvider2.UserID)
                            //{
                            //    isChange = true;
                            //    if (logProvider.UserID == logProvider.User.ID)
                            //    {
                            //        ListUser.Add(logProvider.User.Name);

                            //    }
                            //    if (logProvider2.UserID == logProvider2.User.ID)
                            //    {
                            //        ListUser2.Add(logProvider2.User.Name);

                            //    }
                            //    break;

                            //}

                            if (isChange == true)
                            {
                                string result = string.Join(", ", ListUser);
                                string result2 = string.Join(", ", ListUser2);

                                List0.Insert(0, "");
                                List1.Insert(0, "Provider: " + result + "->");
                                List2.Insert(0, result2);
                            }

                        }
                    }
                    if (log.Name != log2.Name)
                    {
                        isChange = true;
                        var oldLogName = log.Name;
                        var changeLogName = log2.Name;
                        List0.Insert(0, "");
                        List1.Insert(0, "Name: " + oldLogName + "->");
                        List2.Insert(0, changeLogName);
                    }
                    if (log.Project != log2.Project)
                    {
                        isChange = true;
                        var oldLogProject = log.Project;
                        var changeLogProject = log2.Project;
                        List0.Insert(0, "");
                        List1.Insert(0, "Project: " + oldLogProject + "->");
                        List2.Insert(0, changeLogProject);
                    }
                    if (log.StatusID != log2.StatusID)
                    {
                        isChange = true;

                        if (log.StatusID == log.Status.ID)
                        {
                            var oldLogStatusID = log.Status.StatusName;
                            List1.Insert(0, "Status: " + oldLogStatusID + "->");
                        }
                        if (log2.StatusID == log2.Status.ID)
                        {
                            var changeStatusID = log2.Status.StatusName;
                            List2.Insert(0, changeStatusID);
                        }
                        List0.Insert(0, "");
                    }
                    if (log.Remark != log2.Remark)
                    {
                        isChange = true;

                        var oldLogRemark = log.Remark;
                        var changeRemark = log2.Remark;
                        List0.Insert(0, "");
                        List1.Insert(0, "Remark: " + oldLogRemark + "->");
                        List2.Insert(0, changeRemark);
                    }
                    if (log.DueDate != log2.DueDate)
                    {
                        isChange = true;

                        var oldLogDueDate = log.DueDate;
                        var changeDueDate = log2.DueDate;
                        List0.Insert(0, "");
                        List1.Insert(0, "DueDate: " + oldLogDueDate + "->");
                        List2.Insert(0, changeDueDate.ToString());
                    }
                    if (log.UpdateDate != log2.UpdateDate)
                    {
                        isChange = true;
                        var oldLogUpdateDate = log.UpdateDate;
                        var changeLogUpdateDate = log2.UpdateDate;
                        List0.Insert(0, "");
                        List1.Insert(0, "UpdateDate: ");
                        List2.Insert(0, changeLogUpdateDate.ToString());
                    }

                    if (log.UpdateBy != log2.UpdateBy)
                    {
                        isChange = true;
                        var oldLogUpdateBy = log.UpdateBy;
                        var changeLogUpdateBy = log2.UpdateBy;
                        foreach (var u in db.users.ToList())
                        {
                            if (changeLogUpdateBy == u.ID)
                            {
                                List1.Insert(0, "UpdateBy: " + u.Name);
                            }
                            if (oldLogUpdateBy == u.ID)
                            {
                                List2.Insert(0, "");
                            }
                        }
                        List0.Insert(0, "");

                    }
                    else
                    {
                        //isChange = false;
                        var oldLogUpdateBy = log.UpdateBy;
                        var changeLogUpdateBy = log2.UpdateBy;
                        foreach (var u in db.users.ToList())
                        {
                            if (changeLogUpdateBy == u.ID)
                            {
                                List1.Insert(0, "UpdateBy: " + u.Name);
                            }
                            if (oldLogUpdateBy == u.ID)
                            {
                                List2.Insert(0, "");
                            }
                        }
                        List0.Insert(0, "");
                    }
                    if (isChange == true)
                    {
                        List0.Insert(0, "No:" + x);
                        List1.Insert(0, "");
                        List2.Insert(0, "");
                    }
                }
                ViewBag.List0 = List0;
                ViewBag.List1 = List1;
                ViewBag.List2 = List2;

            }
            return View(work);

        }
    }
}




