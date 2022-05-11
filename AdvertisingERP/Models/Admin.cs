using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AdvertisingERP.Models;
using AdvertisingERP.Filter;
using System.Data.SqlClient;

namespace AdvertisingERP.Models
{
    public class Admin : Common
    {
        public string SenderEmailDisplay { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
        public string EncryptKey { get; set; }
        public string TemplateSubject { get; set; }
        public string TempalteBody { get; set; }
        public List<Admin> lstTemplates { get; set; }
        public List<Admin> lstVendor { get; set; }
        public List<Admin> lstSaleOrders { get; set; }
        public List<Admin> lstbill { get; set; }

        public List<Admin> lstbill1 { get; set; }
        public string SelectedFilePath { get; set; }
        public string VendorName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string NatureOfBusiness { get; set; }
        public string ConcernPerson { get; set; }
        public string ConcerPersonContact { get; set; }
        public string Address { get; set; }
        public string VendorId { get; set; }
        public string Subject { get; set; }
        public string SaleOrderNo { get; set; }
        public string SaleOrderNoID { get; set; }
        public string CustomerName { get; set; }
        public string Contact { get; set; }
        public string OrderDate { get; set; }
        public string CssClass { get; set; }
        public string Body { get; set; }
        public string PK_TemplateID { get; set; }
        public string PurchasOrderNo { get; set; }
        public string CreativeName { get; set; }
        public string CampaignNo { get; set; }
        public string BillNo { get;  set; }
        public string InvoiceNo { get; set; }
        public string SalesOrderNo { get; set; }
        public string StartDate { get; set; }
        public string LoginId { get; set; }

        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string ProfilePic { get; set; }
        public string Pk_Id { get; set; }
        

        public DataSet GetDashboardDetails()
        {
            DataSet ds = DBHelper.ExecuteQuery("AdminDashboard");
            return ds;
        }

        public DataSet GetEmailData()
        {
           
            DataSet ds = DBHelper.ExecuteQuery("GetEmail");
            return ds;
        }

        public DataSet GetSenderEmail()
        {

            DataSet ds = DBHelper.ExecuteQuery("GetSenderEmails");
            return ds;
        }
        public DataSet UpdateBillNo()
        {
            SqlParameter[] para ={   new SqlParameter ("@BillNo", BillNo),
                                     new SqlParameter ("@PurchasOrderNo", PurchasOrderNo),
                                 };

            DataSet ds = DBHelper.ExecuteQuery("UpdateBillNo", para);
            return ds;
        }

        public DataSet UpdateInvoiceNo()
        {
            SqlParameter[] para ={   new SqlParameter ("@InvoiceNo", InvoiceNo),
                                     
                                 };

            DataSet ds = DBHelper.ExecuteQuery("UpdatePaymentStatus", para);
            return ds;
        }
        


        #region SaveEmails

        public DataSet SaveEmails()
        {
            SqlParameter[] para ={   new SqlParameter ("@Name", Name),
                                     new SqlParameter ("@Email", Email),
                                     new SqlParameter ("@Description", Description),
                                     new SqlParameter("@AddedBy", AddedBy), };

            DataSet ds = DBHelper.ExecuteQuery("SaveEmailData", para);
            return ds;
        }

        #endregion

        #region EmailTemplate

        public DataSet GetAllTemplates()
        {
            SqlParameter[] para = { new SqlParameter("@PK_TemplateID", PK_TemplateID) };
            DataSet ds = DBHelper.ExecuteQuery("GetAllTemplates", para);
            return ds;
        }

        public DataSet SaveEmailTemplate()
        {
            SqlParameter[] para ={   new SqlParameter ("@TemplateSubject", Subject),
                                     new SqlParameter ("@TemplateBody", Body),
                                     new SqlParameter ("@FilePath", SelectedFilePath),
                                     new SqlParameter("@AddedBy", AddedBy), };
            DataSet ds = DBHelper.ExecuteQuery("SaveEmailTemplate", para);
            return ds;
        }

        #endregion
        
        public DataSet GetUserProfileDetails()
        {
            SqlParameter[] para =
                {
                new SqlParameter("@LoginId",LoginId)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetUserProfileDetails", para);
            return ds;
        }


        public DataSet ChangePassword()
        {
            SqlParameter[] para =
                {
                new SqlParameter("@OldPassword",Password),
                 new SqlParameter("@NewPassword",NewPassword),
                  new SqlParameter("@UpdatedBy",UpdatedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("ChangePassword", para);
            return ds;
        }

        public DataSet UpdateProfile()
        {
            SqlParameter[] para =
                {
                  new SqlParameter("@Pk_Id",Pk_Id),
                 new SqlParameter("@Name",Name),
                  new SqlParameter("@EmailId",Email),
                      new SqlParameter("@ContactNo",MobileNo),
                 new SqlParameter("@LoginId",LoginId),
                  new SqlParameter("@Password",Password),
                     new SqlParameter("@Address",Address),
                      new SqlParameter("@UserImage",ProfilePic),
                 new SqlParameter("@UpdatedBy",UpdatedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("UpdateProfile", para);
            return ds;
        }

        public DataSet UpdateProfilePic()
        {
            SqlParameter[] para = { new SqlParameter("@Pk_Id",Pk_Id ),
                                      new SqlParameter("@UserImage", ProfilePic),
                                       new SqlParameter("@UpdatedBy", UpdatedBy)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("UpdateProfilePic", para);
            return ds;
        }


    }
}