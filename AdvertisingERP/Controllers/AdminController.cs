using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvertisingERP.Models;
using AdvertisingERP.Filter;
using System.Data;
using System.IO;
using System.Net.Mail;

namespace AdvertisingERP.Controllers
{
    public class AdminController : AdminBaseController
    {

        public ActionResult DashBoard()
        {
            Admin model = new Admin();
            try
            {
                DataSet ds = model.GetDashboardDetails();
                if (ds != null && ds.Tables.Count > 0)
                {
                    ViewBag.TotalCustomers = ds.Tables[0].Rows[0]["TotalCustomers"].ToString();
                    ViewBag.TotalVendors = ds.Tables[1].Rows[0]["TotalVendors"].ToString();
                    ViewBag.TotalCampaigns = ds.Tables[2].Rows[0]["TotalCampaigns"].ToString();
                    ViewBag.TotalSalesOrder = ds.Tables[3].Rows[0]["TotalSalesOrder"].ToString();

                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        List<Admin> lstSO = new List<Admin>();
                        foreach (DataRow r in ds.Tables[4].Rows)
                        {
                            Admin obj = new Admin();
                            obj.PurchasOrderNo = r["PurchasOrderNo"].ToString();
                            obj.CampaignNo = r["CampaignNo"].ToString();
                            obj.CreativeName = r["CreativeName"].ToString();

                            lstSO.Add(obj);
                        }
                        model.lstSaleOrders = lstSO;
                    }
                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        List<Admin> lstbill = new List<Admin>();
                        foreach (DataRow r in ds.Tables[5].Rows)
                        {
                            Admin obj = new Admin();
                            obj.InvoiceNo = r["InvoiceNo"].ToString();

                            obj.CreativeName = r["CreativeName"].ToString();
                            obj.StartDate = r["StartDate"].ToString();
                            lstbill.Add(obj);
                        }
                        model.lstbill = lstbill;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        public ActionResult GetSenderEmails()
        {
            Admin model = new Models.Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetSenderEmail();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.SenderEmail = dr["SenderEmail"].ToString();
                    obj.SenderPassword = dr["Password"].ToString();

                    lst.Add(obj);
                }
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        #region SaveEmailIDs

        public ActionResult EmailMaster()
        {
            return View();
        }

        [HttpPost]
        [ActionName("EmailMaster")]
        [OnAction(ButtonName = "btnSaveEmail")]
        public ActionResult SaveEmails(Admin model)
        {
            try
            {
                model.AddedBy = Session["UserID"].ToString();
                DataSet ds = model.SaveEmails();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["EmailData"] = "Email data saved successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["EmailData"] = "ERROR : " + ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("EmailMaster");
        }

        #endregion

        #region EmailTemplate

        public ActionResult EmailTemplate()
        {
            return View();
        }

        [HttpPost]
        [ActionName("EmailTemplate")]
        [OnAction(ButtonName = "btnSaveTemplate")]
        public ActionResult SaveEmailTemplate(Admin model, HttpPostedFileBase postedfile)
        {
            try
            {
                if (postedfile != null)
                {
                    model.SelectedFilePath = "../SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(postedfile.FileName);
                    postedfile.SaveAs(Path.Combine(Server.MapPath(model.SelectedFilePath)));
                }
                model.AddedBy = Session["UserID"].ToString();

                DataSet ds = model.SaveEmailTemplate();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["EmailTemplate"] = "Template saved successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["MSG"].ToString() == "0")
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["EmailTemplate"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("EmailTemplate");
        }

        public ActionResult TemplateList()
        {
            Admin model = new Admin();
            try
            {
                DataSet ds = model.GetAllTemplates();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    List<Admin> lst = new List<Admin>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Admin obj = new Admin();
                        obj.EncryptKey = Crypto.Decrypt(r["PK_TemplateID"].ToString());
                        obj.PK_TemplateID = r["PK_TemplateID"].ToString();
                        obj.TemplateSubject = r["TemplateSubject"].ToString();
                        obj.TempalteBody = r["TemplateBody"].ToString();
                        lst.Add(obj);
                    }
                    model.lstTemplates = lst;
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
        #endregion

        #region SendEmail
        public ActionResult TestEditor()
        {
            return View();
        }
        public ActionResult SendEmail()
        {
            Admin model = new Admin();
            try
            {
                model.SenderEmailDisplay = "contact.afluex@gmail.com";
                //model.SenderEmail = "prakher.afluex@gmail.com";
                //model.SenderPassword = "Baby8542816119";

                List<Admin> lst = new List<Admin>();
                DataSet ds = model.GetEmailData();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Admin obj = new Admin();
                        obj.Name = dr["Name"].ToString();
                        obj.Email = dr["Email"].ToString();
                        obj.Description = dr["Description"].ToString();

                        lst.Add(obj);
                    }
                    model.lstVendor = lst;
                }

                int count4 = 0;
                List<SelectListItem> ddlTemplates = new List<SelectListItem>();
                DataSet dsTemplate = model.GetAllTemplates();
                if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dsTemplate.Tables[0].Rows)
                    {
                        if (count4 == 0)
                        {
                            ddlTemplates.Add(new SelectListItem { Text = "Select Template", Value = "0" });
                        }
                        ddlTemplates.Add(new SelectListItem { Text = r["TemplateSubject"].ToString(), Value = r["PK_TemplateID"].ToString() });
                        count4 = count4 + 1;
                    }
                }
                ViewBag.ddlTemplates = ddlTemplates;
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        public ActionResult TemplateChange(string tid)
        {
            Admin model = new Admin();
            try
            {
                model.PK_TemplateID = tid;
                DataSet ds = model.GetAllTemplates();

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    model.Result = "1";
                    model.SelectedFilePath = ds.Tables[0].Rows[0]["FilePath"].ToString();
                    model.Subject = ds.Tables[0].Rows[0]["TemplateSubject"].ToString();
                    model.Body = ds.Tables[0].Rows[0]["TemplateBody"].ToString();
                }
            }
            catch (Exception ex)
            {
                model.Result = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("SendEmail")]
        [OnAction(ButtonName = "btnSendEmail")]
        public ActionResult SendEmailAction(Admin model)
        {
            try
            {
                int ctrCheck = 0;
                string ctrRowCount = Request["hdRows"].ToString();
                string recipientEmail = "";

                for (int i = 0; i < int.Parse(ctrRowCount); i++)
                {
                    recipientEmail += Request["txtEmail_" + i].ToString() + ";";
                }
                SmtpClient mailServer = new SmtpClient("smtp.gmail.com", 587);
                mailServer.EnableSsl = true;
                //mailServer.Credentials = new System.Net.NetworkCredential(model.SenderEmail, model.SenderPassword);
                mailServer.Credentials = new System.Net.NetworkCredential("contact.afluex@gmail.com", "krishna@9919");

                MailMessage myMail = new MailMessage();
                myMail.Subject = model.Subject;
                myMail.Body = model.Body;
                myMail.From = new MailAddress("contact.afluex@gmail.com");
                myMail.To.Add("supportnow@afluex.com");
                foreach (var emailid in recipientEmail.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    myMail.Bcc.Add(emailid);
                }

                HttpPostedFileBase file = Request.Files["postedfile"];
                if (file != null && file.ContentLength > 0)
                {
                    if (file.ContentLength < 12288000)
                    {
                        string filename = Path.GetFileName(file.FileName);
                        var attachment = new Attachment(file.InputStream, filename);
                        myMail.Attachments.Add(attachment);
                    }
                    else
                    {
                        string uploadFilename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        model.SelectedFilePath = "../EmailAttachments/" + uploadFilename;
                        file.SaveAs(Path.Combine(Server.MapPath(model.SelectedFilePath)));

                        myMail.Body += "Attachment Link : http://erp.afluex.com/EmailAttachments/" + uploadFilename;
                    }
                }
                mailServer.Send(myMail);
                ctrCheck++;
                TempData["Class"] = "alert alert-success";
                TempData["SendEmail"] = "Email sent successfully";
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["SendEmail"] = "ERROR : " + ex.Message;
            }
            return RedirectToAction("SendEmail");
        }
        #endregion

        public ActionResult UpdateBillNo(string BillNo, string PONo)
        {
            Admin model = new Models.Admin();
            List<Admin> lst = new List<Admin>();
            model.BillNo = BillNo;
            model.PurchasOrderNo = PONo;
            DataSet ds = model.UpdateBillNo();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                {
                    model.Result = "1";
                }
                else
                {
                    model.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateSO(string InvoiceNo)
        {
            Admin model = new Models.Admin();
            List<Admin> lst = new List<Admin>();
            model.InvoiceNo = InvoiceNo;

            DataSet ds = model.UpdateInvoiceNo();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                {
                    model.Result = "1";
                }
                else
                {
                    model.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Profile()
        {
            Admin model = new Admin();
            try
            {
                model.AddedBy = Session["LoginID"].ToString();
                DataSet ds = model.GetUserProfileDetails();
                if (ds != null && ds.Tables.Count > 0)
                { 
                    model.Pk_Id = ds.Tables[0].Rows[0]["Pk_Id"].ToString();
                    model.LoginId = ds.Tables[0].Rows[0]["LoginId"].ToString();
                    model.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    model.MobileNo = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                    model.Email = ds.Tables[0].Rows[0]["EmailId"].ToString();
                    model.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    model.ProfilePic = ds.Tables[0].Rows[0]["ProfilePic"].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        [HttpPost]
        [ActionName("Profile")]
        [OnAction(ButtonName = "btnUpdate")]
        public ActionResult UpdateProfile(Admin model, HttpPostedFileBase ProfilePic)
        {
            try
            {

                if (ProfilePic != null)
                {
                    model.ProfilePic = "../ProfilePic/" + Guid.NewGuid() + Path.GetExtension(ProfilePic.FileName);
                    ProfilePic.SaveAs(Path.Combine(Server.MapPath(model.ProfilePic)));
                }

                model.UpdatedBy = Session["UserID"].ToString();
                DataSet ds = model.UpdateProfile();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["msg"] = "Profile updated successfully";
                    }
                    else
                    {
                        TempData["msgerror"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                TempData["msgerror"] = ex.Message;
            }
            return RedirectToAction("Profile", "Admin");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult UpdatePassword(string Password,string NewPassword)
        {
            Admin model = new Admin();
            model.Password = Password;
            model.NewPassword = NewPassword;
            model.UpdatedBy = Session["UserID"].ToString();
            DataSet ds = model.ChangePassword();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                {
                    TempData["msg"] = "Change password successfully";
                }
                else
                {
                    TempData["msgerro"] =ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UpdateProfilePic(string UserId)
        {
            Admin obj = new Admin();
            bool msg = false;
            if (Request.Files.Count > 0)
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files[0];

                string fileName = file.FileName;
                obj.Pk_Id = UserId;
                obj.ProfilePic = "../ProfilePic/" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(Server.MapPath(obj.ProfilePic)));
                obj.UpdatedBy= Session["UserID"].ToString();
                DataSet ds = obj.UpdateProfilePic();

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        msg = true;
                        Session["ProfilePic"] = obj.ProfilePic;

                    }
                    else
                    {
                        msg = false;
                       
                    }
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


    }
}
